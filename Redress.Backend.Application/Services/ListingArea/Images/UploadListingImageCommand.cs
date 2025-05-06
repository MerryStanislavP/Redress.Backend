using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.ListingArea.Images
{
    public class UploadListingImageCommand : IRequest<Guid>
    {
        public ListingImageCreateDto Image { get; set; }
    }

    public class UploadListingImageCommandHandler : IRequestHandler<UploadListingImageCommand, Guid>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public UploadListingImageCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(UploadListingImageCommand request, CancellationToken cancellationToken)
        {
            // Verify that listing exists
            var listingExists = await _context.Listings
                .AnyAsync(l => l.Id == request.Image.ListingId, cancellationToken);

            if (!listingExists)
                throw new KeyNotFoundException($"Listing with ID {request.Image.ListingId} not found");

            var image = _mapper.Map<ListingImage>(request.Image);
            image.CreatedAt = DateTime.UtcNow;

            await _context.ListingImages.AddAsync(image, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return image.Id;
        }
    }
}
