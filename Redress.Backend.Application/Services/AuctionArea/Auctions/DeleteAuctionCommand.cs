using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.AuctionArea.Auctions
{
    public class DeleteAuctionCommand : IRequest, IOwnershipCheck
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // ID пользователя, выполняющего удаление
        
        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == UserId, cancellationToken);

            if (user == null)
                return false;

            // Admin and moderator can delete auctions
            if (user.Role == UserRole.Admin || user.Role == UserRole.Moderator)
                return true;

            // Check if auction belongs to user
            var auction = await context.Auctions
                .Include(a => a.Listing)
                .ThenInclude(l => l.Profile)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(a => a.Id == Id, cancellationToken);

            if (auction == null)
                return false;

            // Listing owner can delete the auction if it's not active
            return auction.Listing?.Profile?.UserId == UserId && auction.EndAt <= DateTime.UtcNow;
        }
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
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (auction == null)
                throw new KeyNotFoundException($"Auction with ID {request.Id} not found");

            // Check if auction is active
            if (auction.EndAt > DateTime.UtcNow)
                throw new InvalidOperationException("Cannot delete an active auction");

            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 