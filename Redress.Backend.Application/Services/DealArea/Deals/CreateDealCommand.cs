using System;
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

namespace Redress.Backend.Application.Services.DealArea.Deals
{
    public class CreateDealCommand : IRequest<Guid>, IOwnershipCheck
    {
        public DealCreateDto Deal { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            // Check if the profile exists and belongs to the user
            var profile = await context.Profiles
                .FirstOrDefaultAsync(p => p.Id == Deal.ProfileId, cancellationToken);

            if (profile == null)
                return false;

            return profile.UserId == UserId;
        }
    }

    public class CreateDealCommandHandler : IRequestHandler<CreateDealCommand, Guid>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public CreateDealCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateDealCommand request, CancellationToken cancellationToken)
        {
            // Verify that listing exists and is available
            var listing = await _context.Listings
                .Include(l => l.Deal)
                .FirstOrDefaultAsync(l => l.Id == request.Deal.ListingId, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {request.Deal.ListingId} not found");

            if (listing.Deal != null)
                throw new InvalidOperationException("This listing is already part of a deal");

            // Verify that profile exists
            var profileExists = await _context.Profiles
                .AnyAsync(p => p.Id == request.Deal.ProfileId, cancellationToken);

            if (!profileExists)
                throw new KeyNotFoundException($"Profile with ID {request.Deal.ProfileId} not found");

            var deal = _mapper.Map<Deal>(request.Deal);
            deal.CreatedAt = DateTime.UtcNow;

            await _context.Deals.AddAsync(deal, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return deal.Id;
        }
    }
}
