﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Redress.Backend.Application.Services.ListingArea.Images
{
    public class UploadListingImageCommand : IRequest<Guid>, IOwnershipCheck
    {
        public IFormFile Image { get; set; }
        public Guid ListingId { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var listing = await context.Listings
                .Include(l => l.Profile)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(l => l.Id == ListingId, cancellationToken);

            if (listing == null)
                return false;

            // Only listing owner can upload images
            return listing.Profile?.UserId == UserId;
        }
    }

    public class UploadListingImageCommandHandler : IRequestHandler<UploadListingImageCommand, Guid>
    {
        private readonly IRedressDbContext _context;
        private readonly IFileService _fileService;

        public UploadListingImageCommandHandler(IRedressDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<Guid> Handle(UploadListingImageCommand request, CancellationToken cancellationToken)
        {
            // Verify that listing exists
            var listing = await _context.Listings
                .FirstOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {request.ListingId} not found");

            // Save the image file
            var imageUrl = await _fileService.SaveFileAsync(request.Image, "listing-images");

            // Create new image record
            var image = new ListingImage
            {
                Name = request.Image.FileName,
                Url = imageUrl,
                CreatedAt = DateTime.UtcNow,
                ListingId = request.ListingId
            };

            await _context.ListingImages.AddAsync(image, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return image.Id;
        }
    }
}
