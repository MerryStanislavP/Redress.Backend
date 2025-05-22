using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.AuctionArea.Bids
{
    public class DeleteBidCommand : IRequest, IOwnershipCheck
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            // Get the bid with its profile
            var bid = await context.Bids
                .Include(b => b.Profile)
                .FirstOrDefaultAsync(b => b.Id == Id, cancellationToken);

            if (bid == null)
                return false;

            // User can only delete their own bids
            return bid.Profile.UserId == UserId;
        }
    }

    public class DeleteBidCommandHandler : IRequestHandler<DeleteBidCommand>
    {
        private readonly IRedressDbContext _context;

        public DeleteBidCommandHandler(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteBidCommand request, CancellationToken cancellationToken)
        {
            var bid = await _context.Bids
                .Include(b => b.Auction)
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (bid == null)
                throw new KeyNotFoundException($"Bid with ID {request.Id} not found");

            // Check if auction is still active
            if (bid.Auction.EndAt <= DateTime.UtcNow)
                throw new InvalidOperationException("Cannot delete bid from ended auction");

            _context.Bids.Remove(bid);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 