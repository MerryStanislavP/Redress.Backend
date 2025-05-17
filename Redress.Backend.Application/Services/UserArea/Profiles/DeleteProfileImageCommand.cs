using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.UserArea.Profiles
{
    public class DeleteProfileImageCommand : IRequest, IOwnershipCheck
    {
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var profile = await context.Profiles
                .FirstOrDefaultAsync(p => p.Id == ProfileId, cancellationToken);

            if (profile == null)
                return false;

            // User can only delete their own profile image
            return profile.UserId == UserId;
        }
    }

    public class DeleteProfileImageCommandHandler : IRequestHandler<DeleteProfileImageCommand>
    {
        private readonly IRedressDbContext _context;

        public DeleteProfileImageCommandHandler(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteProfileImageCommand request, CancellationToken cancellationToken)
        {
            var profile = await _context.Profiles
                .Include(p => p.ProfileImage)
                .FirstOrDefaultAsync(p => p.Id == request.ProfileId, cancellationToken);

            if (profile == null)
                throw new KeyNotFoundException($"Profile with ID {request.ProfileId} not found");

            if (profile.ProfileImage == null)
                throw new InvalidOperationException("Profile does not have an image to delete");

            // TODO: Delete image from cloud storage
            // var imageUrl = profile.ProfileImage.Url;
            // await _cloudStorageService.DeleteFileAsync(imageUrl);

            _context.ProfileImages.Remove(profile.ProfileImage);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 