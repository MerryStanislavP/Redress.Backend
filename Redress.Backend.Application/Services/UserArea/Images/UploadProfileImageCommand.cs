using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Entities;

namespace Redress.Backend.Application.Services.UserArea.Images
{
    public class UploadProfileImageCommand : IRequest<Guid>, IOwnershipCheck
    {
        public IFormFile Image { get; set; }
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var profile = await context.Profiles
                .FirstOrDefaultAsync(p => p.Id == ProfileId, cancellationToken);

            if (profile == null)
                return false;

            // User can only upload images for their own profile
            return profile.UserId == UserId;
        }
    }

    public class UploadProfileImageCommandHandler : IRequestHandler<UploadProfileImageCommand, Guid>
    {
        private readonly IRedressDbContext _context;
        private readonly IFileService _fileService;

        public UploadProfileImageCommandHandler(IRedressDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<Guid> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
        {
            var profile = await _context.Profiles
                .Include(p => p.ProfileImage)
                .FirstOrDefaultAsync(p => p.Id == request.ProfileId, cancellationToken);

            if (profile == null)
                throw new KeyNotFoundException($"Profile with ID {request.ProfileId} not found");

            // Delete existing image if any
            if (profile.ProfileImage != null)
            {
                await _fileService.DeleteFileAsync(profile.ProfileImage.Url);
                _context.ProfileImages.Remove(profile.ProfileImage);
            }

            // Save new image
            var imageUrl = await _fileService.SaveFileAsync(request.Image, "profile-images");

            var profileImage = new ProfileImage
            {
                Name = request.Image.FileName,
                Url = imageUrl,
                CreatedAt = DateTime.UtcNow,
                ProfileId = request.ProfileId
            };

            await _context.ProfileImages.AddAsync(profileImage, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return profileImage.Id;
        }
    }
}