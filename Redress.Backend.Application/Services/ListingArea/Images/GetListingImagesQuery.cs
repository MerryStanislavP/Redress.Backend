using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.ListingArea.Images
{
    public class GetListingImagesQuery : IRequest<List<ListingImageDto>>
    {
        public Guid ListingId { get; set; }
        public Guid UserId { get; set; }
    }

    public class GetListingImagesQueryHandler : IRequestHandler<GetListingImagesQuery, List<ListingImageDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetListingImagesQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ListingImageDto>> Handle(GetListingImagesQuery request, CancellationToken cancellationToken)
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

            // Получаем изображения, отсортированные по дате создания (от старых к новым)
            var images = await _context.ListingImages
                .Where(i => i.ListingId == request.ListingId)
                .OrderBy(i => i.CreatedAt)
                .Select(i => new ListingImageDto 
                { 
                    Id = i.Id,
                    Url = i.Url
                })
                .ToListAsync(cancellationToken);

            return images;
        }
    }
} 