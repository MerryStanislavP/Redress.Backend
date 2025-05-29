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

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class GetListingsBySexQuery : IRequest<PaginatedList<ListingDto>>
    {
        public Sex Sex { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 28;
        public Guid UserId { get; set; }
    }

    public class GetListingsBySexQueryHandler : IRequestHandler<GetListingsBySexQuery, PaginatedList<ListingDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public GetListingsBySexQueryHandler(IRedressDbContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<PaginatedList<ListingDto>> Handle(GetListingsBySexQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            var query = _context.Listings
                .Include(l => l.Category)
                .Include(l => l.ListingImages)
                .Where(l => l.Category.Sex == request.Sex && l.Status == ListingStatus.Active)
                .OrderByDescending(l => l.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<ListingDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            // Для каждого объявления получаем signed URL для главного изображения
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.Url))
                    item.Url = await _fileService.GetFileUrlAsync(item.Url);
            }

            return new PaginatedList<ListingDto>(items, request.Page, request.PageSize, totalCount);
        }
    }
} 