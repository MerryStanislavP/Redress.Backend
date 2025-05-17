using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.UserArea.Images
{
    public class UploadProfileImageCommand : IRequest<Guid>
    {
        public ProfileImageCreateDto ProfileImage { get; set; }
    }

    public class UploadProfileImageCommandHandler : IRequestHandler<UploadProfileImageCommand, Guid>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public UploadProfileImageCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
        {
            // Verify that profile exists
            var profileExists = await _context.Profiles
                .AnyAsync(p => p.Id == request.ProfileImage.ProfileId, cancellationToken);

            if (!profileExists)
                throw new KeyNotFoundException($"Profile with ID {request.ProfileImage.ProfileId} not found");

            // Remove existing profile image if any
            var existingImage = await _context.ProfileImages
                .FirstOrDefaultAsync(pi => pi.ProfileId == request.ProfileImage.ProfileId, cancellationToken);

            if (existingImage != null)
            {
                _context.ProfileImages.Remove(existingImage);
            }

            var image = _mapper.Map<ProfileImage>(request.ProfileImage);
            image.CreatedAt = DateTime.UtcNow;

            await _context.ProfileImages.AddAsync(image, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return image.Id;
        }
    }
}
