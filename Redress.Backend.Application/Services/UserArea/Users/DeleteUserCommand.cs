using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Enums;
using Redress.Backend.Application.Common.Behavior;

namespace Redress.Backend.Application.Services.UserArea.Users
{
    public class DeleteUserCommand : IRequest, IRequireRole
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public UserRole RequiredRole => UserRole.Admin;
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IRedressDbContext _context;

        public DeleteUserCommandHandler(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Profile)
                .ThenInclude(p => p.Listings)
                .ThenInclude(l => l.Deal)
                .Include(u => u.Profile)
                .ThenInclude(p => p.Listings)
                .ThenInclude(l => l.Auction)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.Id} not found");

            // Check if user has any active deals
            var hasActiveDeals = user.Profile?.Listings
                .Any(l => l.Deal != null && l.Deal.Status == Domain.Enums.DealStatus.Pending) ?? false;

            if (hasActiveDeals)
                throw new InvalidOperationException("Cannot delete user with active deals");

            // Check if user has any active auctions
            var hasActiveAuctions = user.Profile?.Listings
                .Any(l => l.Auction != null && l.Auction.EndAt > DateTime.UtcNow) ?? false;

            if (hasActiveAuctions)
                throw new InvalidOperationException("Cannot delete user with active auctions");

            // Remove the user and related entities
            if (user.Profile != null)
            {
                _context.Profiles.Remove(user.Profile);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 