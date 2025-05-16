using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.ListingArea.Images
{
    public class DeleteListingImageCommand : IRequest, IOwnershipCheck
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var listingImage = await context.ListingImages
                .Include(l => l.Listing)
                    .ThenInclude(p => p.Profile)
                        .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(l => l.Id == Id, cancellationToken);

            if (listingImage == null)
                return false;

            // Проверяем, является ли пользователь владельцем листинга
            if (listingImage.Listing?.Profile?.User?.Role == UserRole.Admin)
            {
                return true;
            }
            return listingImage.Listing?.Profile?.UserId == UserId;
        }
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
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (image == null)
                throw new KeyNotFoundException($"Listing image with ID {request.Id} not found");

            // Check if listing is part of an active deal
            if (image.Listing?.Deal != null && image.Listing.Deal.Status == DealStatus.Pending)
                throw new InvalidOperationException("Cannot delete image from a listing that is part of an active deal");

            // Check if listing is part of an active auction
            if (image.Listing?.Auction != null && image.Listing.Auction.EndAt > DateTime.UtcNow)
                throw new InvalidOperationException("Cannot delete image from a listing that is part of an active auction");

            _context.ListingImages.Remove(image);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}