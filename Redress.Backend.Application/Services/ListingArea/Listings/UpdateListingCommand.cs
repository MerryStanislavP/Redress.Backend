using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class UpdateListingCommand : IRequest, IOwnershipCheck
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ListingUpdateDto UpdateDto { get; set; }
        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var listing = await context.Listings
                .Include(l => l.Profile)
                    .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(l => l.Id == Id, cancellationToken);

            if (listing == null)
                return false;

            return listing.Profile?.UserId == UserId;
        }
    }

    public class UpdateListingCommandHandler : IRequestHandler<UpdateListingCommand>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public UpdateListingCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(UpdateListingCommand request, CancellationToken cancellationToken)
        {
            var listing = await _context.Listings
                .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {request.Id} not found");

            // If category is being updated, verify it exists
            if (request.UpdateDto.CategoryId.HasValue)
            {
                var categoryExists = await _context.Categories
                    .AnyAsync(c => c.Id == request.UpdateDto.CategoryId.Value, cancellationToken);

                if (!categoryExists)
                    throw new KeyNotFoundException($"Category with ID {request.UpdateDto.CategoryId} not found");
            }

            // Update only provided fields
            if (!string.IsNullOrEmpty(request.UpdateDto.Title))
                listing.Title = request.UpdateDto.Title;

            if (!string.IsNullOrEmpty(request.UpdateDto.Description))
                listing.Description = request.UpdateDto.Description;

            if (request.UpdateDto.Price.HasValue)
                listing.Price = request.UpdateDto.Price.Value;

            if (request.UpdateDto.CategoryId.HasValue)
                listing.CategoryId = request.UpdateDto.CategoryId.Value;

            if (request.UpdateDto.Latitude.HasValue)
                listing.Latitude = request.UpdateDto.Latitude.Value;

            if (request.UpdateDto.Longitude.HasValue)
                listing.Longitude = request.UpdateDto.Longitude.Value;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
