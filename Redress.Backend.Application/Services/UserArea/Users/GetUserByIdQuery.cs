using MediatR;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Enums;
using Redress.Backend.Application.Common.Behavior;

namespace Redress.Backend.Application.Services.UserArea.Users
{
    public class GetUserByIdQuery : IRequest<UserDto>, IRequireRole
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public UserRole RequiredRole => UserRole.Admin;
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.Id} not found");

            return _mapper.Map<UserDto>(user);
        }
    }
}
