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

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class CreateListingCommand : IRequest<Guid>
    {
        public ListingCreateDto Listing { get; set; }
    }

    public class CreateListingCommandHandler : IRequestHandler<CreateListingCommand, Guid>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public CreateListingCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateListingCommand request, CancellationToken cancellationToken)
        {
            var listing = _mapper.Map<Listing>(request.Listing);
            
            // Verify that profile exists
            var profileExists = await _context.Profiles
                .AnyAsync(p => p.Id == request.Listing.ProfileId, cancellationToken);
            
            if (!profileExists)
                throw new KeyNotFoundException($"Profile with ID {request.Listing.ProfileId} not found");

            // Verify that category exists
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.Listing.CategoryId, cancellationToken);
            
            if (!categoryExists)
                throw new KeyNotFoundException($"Category with ID {request.Listing.CategoryId} not found");

            await _context.Listings.AddAsync(listing, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return listing.Id;
        }
    }
}
