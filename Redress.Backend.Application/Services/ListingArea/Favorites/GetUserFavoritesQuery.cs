using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using Redress.Backend.Domain.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Application.Common.Models;

namespace Redress.Backend.Application.Services.ListingArea.Favorites
{
    public class GetUserFavoritesQuery : IRequest<PaginatedList<ListingDto>>
    {
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; } // ID пользователя, запрашивающего данные
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetUserFavoritesQueryHandler : IRequestHandler<GetUserFavoritesQuery, PaginatedList<ListingDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetUserFavoritesQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ListingDto>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
        {
            // Получаем пользователя для проверки прав
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            // Проверяем, что профиль принадлежит пользователю или пользователь - админ/модератор
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.Id == request.ProfileId, cancellationToken);

            if (profile == null)
                throw new KeyNotFoundException($"Profile with ID {request.ProfileId} not found");

            bool isOwner = profile.UserId == request.UserId;
            bool isAdminOrModerator = user.Role == UserRole.Admin || user.Role == UserRole.Moderator;

            if (!isOwner && !isAdminOrModerator)
                throw new UnauthorizedAccessException("You don't have permission to access these favorites");

            var query = _context.Favorites
                .Where(f => f.ProfileId == request.ProfileId)
                .Select(f => f.Listing)
                .OrderByDescending(l => l.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<ListingDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PaginatedList<ListingDto>(items, request.Page, request.PageSize, totalCount);
        }
    }
}
