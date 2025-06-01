using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.UserArea.Users
{
    public class GetAllUsersQuery : IRequest<PaginatedList<UserDto>>, IRequireRole
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid UserId { get; set; }

        public UserRole RequiredRole => UserRole.Admin;
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PaginatedList<UserDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Users
                .OrderByDescending(u => u.LastLoginAt);

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