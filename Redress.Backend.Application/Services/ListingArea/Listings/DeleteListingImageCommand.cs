using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class DeleteListingImageCommand : IRequest
    {
        public Guid ImageId { get; set; }
    }

    public class DeleteListingImageCommandHandler : IRequestHandler<DeleteListingImageCommand>
    {
        private readonly IRedressDbContext _context;

        public DeleteListingImageCommandHandler(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteListingImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _context.ListingImages
                .Include(i => i.Listing)
                .FirstOrDefaultAsync(i => i.Id == request.ImageId, cancellationToken);

            if (image == null)
                throw new KeyNotFoundException($"Listing image with ID {request.ImageId} not found");

            // Check if listing is part of an active deal
            if (image.Listing?.Deal != null && image.Listing.Deal.Status == Domain.Enums.DealStatus.Pending)
                throw new InvalidOperationException("Cannot delete image from a listing that is part of an active deal");

            // Check if listing is part of an active auction
            if (image.Listing?.Auction != null && image.Listing.Auction.EndAt > DateTime.UtcNow)
                throw new InvalidOperationException("Cannot delete image from a listing that is part of an active auction");

            _context.ListingImages.Remove(image);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 