using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTO;
using Redress.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.ListingArea.Favorites
{
    public class AddToFavoritesCommand : IRequest<Guid>, IOwnershipCheck
    {
        public FavoriteCreateDto Favorite { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            // Get the profile
            var profile = await context.Profiles
                .FirstOrDefaultAsync(p => p.Id == Favorite.ProfileId, cancellationToken);

            if (profile == null)
                return false;

            // User can only add to favorites for their own profile
            return profile.UserId == UserId;
        }
    }

    public class AddToFavoritesCommandHandler : IRequestHandler<AddToFavoritesCommand, Guid>
    {
        private readonly IRedressDbContext _context;

        public AddToFavoritesCommandHandler(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(AddToFavoritesCommand request, CancellationToken cancellationToken)
        {
            // Check if profile exists
            var profileExists = await _context.Profiles
                .AnyAsync(p => p.Id == request.Favorite.ProfileId, cancellationToken);

            if (!profileExists)
                throw new KeyNotFoundException($"Profile with ID {request.Favorite.ProfileId} not found");

            // Check if listing exists
            var listingExists = await _context.Listings
                .AnyAsync(l => l.Id == request.Favorite.ListingId, cancellationToken);

            if (!listingExists)
                throw new KeyNotFoundException($"Listing with ID {request.Favorite.ListingId} not found");

            // Check if favorite already exists
            var favoriteExists = await _context.Favorites
                .AnyAsync(f => f.ProfileId == request.Favorite.ProfileId && 
                             f.ListingId == request.Favorite.ListingId, 
                             cancellationToken);

            if (favoriteExists)
                throw new InvalidOperationException("This listing is already in favorites");

            var favorite = new Favorite
            {
                ProfileId = request.Favorite.ProfileId,
                ListingId = request.Favorite.ListingId
            };

            await _context.Favorites.AddAsync(favorite, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return favorite.Id;
        }
    }
}
