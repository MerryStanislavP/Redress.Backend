using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Enums;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Redress.Backend.Application.Common.Behavior;

namespace Redress.Backend.Application.Services.UserArea.Users
{
    public class GetUsersByRoleQuery : IRequest<PaginatedList<UserDto>>, IRequireRole
    {
        public UserRole Role { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid UserId { get; set; } // ID пользователя, который делает запрос

        public UserRole RequiredRole => UserRole.Admin;
    }

    public class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQuery, PaginatedList<UserDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetUsersByRoleQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<UserDto>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
        {
            // Проверяем, что запрашивающий пользователь существует и является админом
            var requestingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (requestingUser == null)
                throw new UnauthorizedAccessException("User not found");

            if (requestingUser.Role != UserRole.Admin)
                throw new UnauthorizedAccessException("Only administrators can view users by role");

            var query = _context.Users
                .Where(u => u.Role == request.Role)
                .OrderBy(u => u.Username);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PaginatedList<UserDto>(items, request.Page, request.PageSize, totalCount);
        }
    }
} 