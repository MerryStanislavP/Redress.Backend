using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class GetAllListingsQuery : IRequest<PaginatedList<ListingDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid UserId { get; set; }

    }

    public class GetAllListingsQueryHandler : IRequestHandler<GetAllListingsQuery, PaginatedList<ListingDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public GetAllListingsQueryHandler(IRedressDbContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<PaginatedList<ListingDto>> Handle(GetAllListingsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Listings
                .Include(l => l.ListingImages)
                .Include(l => l.Category)
                .Include(l => l.Profile)
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