using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.UserArea.Profiles
{
    public class GetProfileByUserIdQuery : IRequest<ProfileDto>
    {
        public Guid UserId { get; set; }
    }

    public class GetProfileByUserIdQueryHandler : IRequestHandler<GetProfileByUserIdQuery, ProfileDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetProfileByUserIdQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProfileDto> Handle(GetProfileByUserIdQuery request, CancellationToken cancellationToken)
        {
            var profile = await _context.Profiles
                .Include(p => p.ProfileImage)
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

            if (profile == null)
                throw new KeyNotFoundException($"Profile for user with ID {request.UserId} not found");

            return _mapper.Map<ProfileDto>(profile);
        }
    }
}
