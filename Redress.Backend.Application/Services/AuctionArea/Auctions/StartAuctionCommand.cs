using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Domain.Entities;
using Redress.Backend.Domain.Enums;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.AuctionArea.Auctions
{
    public class StartAuctionCommand : IRequest<Guid>, IOwnershipCheck
    {
        public Guid ListingId { get; set; }
        public AuctionCreateDto Auction { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var listing = await context.Listings
                .Include(l => l.Profile)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(l => l.Id == ListingId, cancellationToken);

            if (listing == null)
                return false;

            // User must be the owner of the listing to start an auction
            return listing.Profile?.UserId == UserId;
        }
    }

    public class StartAuctionCommandHandler : IRequestHandler<StartAuctionCommand, Guid>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public StartAuctionCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(StartAuctionCommand request, CancellationToken cancellationToken)
        {
            var listing = await _context.Listings
                .Include(l => l.Auction)
                .FirstOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {request.ListingId} not found");

            if (listing.Auction != null)
                throw new InvalidOperationException("This listing already has an auction");

            if (listing.Status != ListingStatus.Active)
                throw new InvalidOperationException("Cannot create auction for inactive listing");

            var auction = _mapper.Map<Auction>(request.Auction);
            auction.ListingId = request.ListingId;
            auction.CreatedAt = DateTime.UtcNow;

            await _context.Auctions.AddAsync(auction, cancellationToken);
            
            // Update listing to indicate it's an auction
            listing.IsAuction = true;
            
            await _context.SaveChangesAsync(cancellationToken);

            return auction.Id;
        }
    }
}
