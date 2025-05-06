using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.AuctionArea.Auctions
{
    public class DeleteAuctionCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteAuctionCommandHandler : IRequestHandler<DeleteAuctionCommand>
    {
        private readonly IRedressDbContext _context;

        public DeleteAuctionCommandHandler(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
        {
            var auction = await _context.Auctions
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (auction == null)
                throw new KeyNotFoundException($"Auction with ID {request.Id} not found");

            // Check if auction is active
            if (auction.EndAt > DateTime.UtcNow)
                throw new InvalidOperationException("Cannot delete an active auction");

            // Remove related bids
            if (auction.Bids.Any())
            {
                _context.Bids.RemoveRange(auction.Bids);
            }

            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 