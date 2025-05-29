using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Redress.Backend.Application.Services.UserArea.Profiles
{
    public class GetUserProfileQuery : IRequest<ProfileDetailsDto>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, ProfileDetailsDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public GetUserProfileQueryHandler(IRedressDbContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ProfileDetailsDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await _context.Profiles
                .Include(p => p.ProfileImage)
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

            if (profile == null)
                throw new KeyNotFoundException($"Profile for user with ID {request.UserId} not found");

            var dto = _mapper.Map<ProfileDetailsDto>(profile);
            if (dto.ProfileImage != null && !string.IsNullOrEmpty(dto.ProfileImage.Url))
            {
                dto.ProfileImage.Url = await _fileService.GetFileUrlAsync(dto.ProfileImage.Url);
            }
            return dto;
        }
    }
} 