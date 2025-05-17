using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.UserArea.Images
{
    public class DeleteProfileImageCommand : IRequest, IOwnershipCheck
    {
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == UserId, cancellationToken);

            if (user == null)
                return false;

            // Admin can delete any profile image
            if (user.Role == UserRole.Admin)
                return true;

            // Get the profile
            var profile = await context.Profiles
                .FirstOrDefaultAsync(p => p.Id == ProfileId, cancellationToken);

            if (profile == null)
                return false;

            // The user can only delete their own profile image
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

            _context.ProfileImages.Remove(profile.ProfileImage);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}