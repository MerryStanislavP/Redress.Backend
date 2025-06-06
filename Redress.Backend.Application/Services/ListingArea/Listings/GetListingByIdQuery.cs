﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class GetListingByIdQuery : IRequest<ListingDetailsDto>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }

    public class GetListingByIdQueryHandler : IRequestHandler<GetListingByIdQuery, ListingDetailsDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public GetListingByIdQueryHandler(IRedressDbContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ListingDetailsDto> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists)
                throw new KeyNotFoundException($"User with ID {request.UserId} not found");

            var listing = await _context.Listings
                .Include(l => l.Category)
                .Include(l => l.Profile)
                .Include(l => l.ListingImages)
                .Include(l => l.Auction)
                .Include(l => l.Deal)
                .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {request.Id} not found");

            var dto = _mapper.Map<ListingDetailsDto>(listing);
            foreach (var img in dto.Images)
            {
                img.Url = await _fileService.GetFileUrlAsync(img.Url);
            }
            return dto;
        }
    }
}
