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

        public GetProfileByIdQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProfileDetailsDto> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var profile = await _context.Profiles
                .Include(p => p.ProfileImage)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (profile == null)
                throw new KeyNotFoundException($"Profile with ID {request.Id} not found");

            return _mapper.Map<ProfileDetailsDto>(profile);
        }
    }
} 