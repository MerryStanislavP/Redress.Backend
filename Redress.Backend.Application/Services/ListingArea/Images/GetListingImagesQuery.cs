using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.ListingArea.Images
{
    public class GetListingImagesQuery : IRequest<PaginatedList<ListingImageDto>>
    {
        public Guid ListingId { get; set; }
        public Guid UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 3;
    }

    public class GetListingImagesQueryHandler : IRequestHandler<GetListingImagesQuery, PaginatedList<ListingImageDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetListingImagesQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ListingImageDto>> Handle(GetListingImagesQuery request, CancellationToken cancellationToken)
        {
            // Проверяем существование пользователя
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            // Проверяем существование листинга и получаем его с профилем
            var listing = await _context.Listings
                .Include(l => l.Profile)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {request.ListingId} not found");

            // Проверяем права доступа:
            // 1. Админ может видеть все изображения
            // 2. Владелец листинга может видеть свои изображения
            // 3. Все пользователи могут видеть изображения активных листингов
            bool hasAccess = user.Role == UserRole.Admin || 
                           listing.Profile?.UserId == request.UserId ||
                           listing.Status == ListingStatus.Active;

            if (!hasAccess)
                throw new UnauthorizedAccessException("You don't have permission to view these images");

            // Получаем изображения, отсортированные по дате создания
            var query = _context.ListingImages
                .Where(i => i.ListingId == request.ListingId)
                .OrderBy(i => i.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<ListingImageDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PaginatedList<ListingImageDto>(items, request.Page, request.PageSize, totalCount);
        }
    }
} 