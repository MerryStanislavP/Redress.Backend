using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Redress.Backend.Application.Services.UserArea.Profiles
{
    public class GetProfileByIdQuery : IRequest<ProfileDetailsDto>
    {
        public Guid Id { get; set; }
    }

    public class GetProfileByIdQueryHandler : IRequestHandler<GetProfileByIdQuery, ProfileDetailsDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public GetProfileByIdQueryHandler(IRedressDbContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ProfileDetailsDto> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var profile = await _context.Profiles
                .Include(p => p.ProfileImage)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (profile == null)
                throw new KeyNotFoundException($"Profile with ID {request.Id} not found");

            var dto = _mapper.Map<ProfileDetailsDto>(profile);
            if (dto.ProfileImage != null && !string.IsNullOrEmpty(dto.ProfileImage.Url))
            {
                dto.ProfileImage.Url = await _fileService.GetFileUrlAsync(dto.ProfileImage.Url);
            }
            return dto;
        }
    }
} 