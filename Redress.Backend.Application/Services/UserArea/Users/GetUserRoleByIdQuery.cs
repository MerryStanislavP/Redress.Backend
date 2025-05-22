using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.Enums;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;

namespace Redress.Backend.Application.Services.UserArea.Users
{
    public class GetUserRoleByIdQuery : IRequest<UserRoleDto>
    {
        public Guid Id { get; set; }
    }
    public class GetUserRoleByIdQueryHandler : IRequestHandler<GetUserRoleByIdQuery, UserRoleDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetUserRoleByIdQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserRoleDto> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.Id} not found");

            return _mapper.Map<UserRoleDto>(user);
        }
    }
}
