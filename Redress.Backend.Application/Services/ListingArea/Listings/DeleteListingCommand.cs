using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class DeleteListingCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteListingCommandHandler : IRequestHandler<DeleteListingCommand>
    {
        private readonly IRedressDbContext _context;

        public DeleteListingCommandHandler(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteListingCommand request, CancellationToken cancellationToken)
        {
            var listing = await _context.Listings
                .Include(l => l.ListingImages)
                .Include(l => l.Favorites)
                .Include(l => l.Auction)
                .Include(l => l.Deal)
                .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {request.Id} not found");

            // Check if listing is part of an active deal
            if (listing.Deal != null && listing.Deal.Status == Domain.Enums.DealStatus.Pending)
                throw new InvalidOperationException("Cannot delete listing that is part of an active deal");

            // Check if listing is part of an active auction
            if (listing.Auction != null && listing.Auction.EndAt > DateTime.UtcNow)
                throw new InvalidOperationException("Cannot delete listing that is part of an active auction");

            // Remove related entities
            _context.ListingImages.RemoveRange(listing.ListingImages);
            _context.Favorites.RemoveRange(listing.Favorites);
            
            if (listing.Auction != null)
                _context.Auctions.Remove(listing.Auction);
            
            if (listing.Deal != null)
                _context.Deals.Remove(listing.Deal);

            // Remove the listing itself
            _context.Listings.Remove(listing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 