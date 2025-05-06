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

namespace Redress.Backend.Application.Services.AuctionArea.Auctions
{
    public class StartAuctionCommand : IRequest<Guid>
    {
        public AuctionCreateDto Auction { get; set; }
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
            // Verify that listing exists and is not already an auction
            var listing = await _context.Listings
                .Include(l => l.Auction)
                .FirstOrDefaultAsync(l => l.Id == request.Auction.ListingId, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {request.Auction.ListingId} not found");

            if (listing.Auction != null)
                throw new InvalidOperationException("This listing already has an auction");

            var auction = _mapper.Map<Auction>(request.Auction);
            auction.CreatedAt = DateTime.UtcNow;

            await _context.Auctions.AddAsync(auction, cancellationToken);
            
            // Update listing to indicate it's an auction
            listing.IsAuction = true;
            
            await _context.SaveChangesAsync(cancellationToken);

            return auction.Id;
        }
    }
}
