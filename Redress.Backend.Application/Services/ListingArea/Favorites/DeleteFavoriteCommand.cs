using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.ListingArea.Favorites
{
    public class DeleteFavoriteCommand : IRequest, IOwnershipCheck
    {
        public Guid ProfileId { get; set; }
        public Guid ListingId { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            // Get the profile
            var profile = await context.Profiles
                .FirstOrDefaultAsync(p => p.Id == ProfileId, cancellationToken);

            if (profile == null)
                return false;

            // User can only delete favorites from their own profile
            return profile.UserId == UserId;
        }
    }

    public class DeleteFavoriteCommandHandler : IRequestHandler<DeleteFavoriteCommand>
    {
        private readonly IRedressDbContext _context;

        public DeleteFavoriteCommandHandler(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteFavoriteCommand request, CancellationToken cancellationToken)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => 
                    f.ProfileId == request.ProfileId && 
                    f.ListingId == request.ListingId, 
                    cancellationToken);

            if (favorite == null)
                throw new KeyNotFoundException($"Favorite not found for profile {request.ProfileId} and listing {request.ListingId}");

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 