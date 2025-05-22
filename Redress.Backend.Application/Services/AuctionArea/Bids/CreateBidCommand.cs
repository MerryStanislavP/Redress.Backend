using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.AuctionArea.Bids
{
    public class CreateBidCommand : IRequest<Guid>, IOwnershipCheck
    {
        public BidCreateDto Bid { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            // Get the profile
            var profile = await context.Profiles
                .FirstOrDefaultAsync(p => p.Id == Bid.ProfileId, cancellationToken);

            if (profile == null)
                return false;

            // User can only create bids for their own profile
            return profile.UserId == UserId;
        }
    }

    public class CreateBidCommandHandler : IRequestHandler<CreateBidCommand, Guid>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public CreateBidCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateBidCommand request, CancellationToken cancellationToken)
        {
            // Verify that auction exists and is active
            var auction = await _context.Auctions
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == request.Bid.AuctionId, cancellationToken);

            if (auction == null)
                throw new KeyNotFoundException($"Auction with ID {request.Bid.AuctionId} not found");

            if (auction.EndAt <= DateTime.UtcNow)
                throw new InvalidOperationException("Cannot place bid on ended auction");

            // Verify that profile exists
            var profileExists = await _context.Profiles
                .AnyAsync(p => p.Id == request.Bid.ProfileId, cancellationToken);

            if (!profileExists)
                throw new KeyNotFoundException($"Profile with ID {request.Bid.ProfileId} not found");

            // Check if bid amount is valid
            var highestBid = auction.Bids.Max(b => b.Amount);
            var minBidAmount = Math.Max(auction.StartPrice, highestBid + auction.MinStep);

            if (request.Bid.Amount < minBidAmount)
                throw new InvalidOperationException($"Bid amount must be at least {minBidAmount}");

            var bid = _mapper.Map<Bid>(request.Bid);
            bid.CreatedAt = DateTime.UtcNow;

            await _context.Bids.AddAsync(bid, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return bid.Id;
        }
    }
} 