using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class DeleteListingImageCommand : IRequest, IOwnershipCheck
    {
        public Guid ImageId { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var image = await context.ListingImages
                .Include(i => i.Listing)
                .ThenInclude(l => l.Profile)
                .FirstOrDefaultAsync(i => i.Id == ImageId, cancellationToken);

            if (image == null)
                return false;

            // User can only delete images from their own listings
            return image.Listing?.Profile?.UserId == UserId;
        }
    }

    public class DeleteListingImageCommandHandler : IRequestHandler<DeleteListingImageCommand>
    {
        private readonly IRedressDbContext _context;
        private readonly IFileService _fileService;

        public DeleteListingImageCommandHandler(IRedressDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task Handle(DeleteListingImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _context.ListingImages
                .FirstOrDefaultAsync(i => i.Id == request.ImageId, cancellationToken);

            if (image == null)
                throw new KeyNotFoundException($"Image with ID {request.ImageId} not found");

            // Delete the file from storage
            await _fileService.DeleteFileAsync(image.Url);

            // Remove the image record from database
            _context.ListingImages.Remove(image);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 