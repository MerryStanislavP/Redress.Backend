using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.UserArea.Images
{
    public class GetProfileImageCommand : IRequest<string>, IOwnershipCheck
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

    public class GetProfileImageCommandHandler : IRequestHandler<GetProfileImageCommand, string>
    {
        private readonly IRedressDbContext _context;
        private readonly IFileService _fileService;

        public GetProfileImageCommandHandler(IRedressDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<string> Handle(GetProfileImageCommand request, CancellationToken cancellationToken)
        {
            var profile = await _context.Profiles
                .Include(p => p.ProfileImage)
                .FirstOrDefaultAsync(p => p.Id == request.ProfileId, cancellationToken);

            if (profile == null || profile.ProfileImage == null || string.IsNullOrEmpty(profile.ProfileImage.Url))
                throw new KeyNotFoundException($"Profile or profile image not found for profileId {request.ProfileId}");

            var signedUrl = await _fileService.GetFileUrlAsync(profile.ProfileImage.Url);
            return signedUrl;
        }
    }
} 