using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Common.Models;
using AutoMapper.QueryableExtensions;

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class GetListingsByPriceRangeQuery : IRequest<PaginatedList<ListingDto>>
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid UserId { get; set; }
    }

    public class GetListingsByPriceRangeQueryHandler : IRequestHandler<GetListingsByPriceRangeQuery, PaginatedList<ListingDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetListingsByPriceRangeQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ListingDto>> Handle(GetListingsByPriceRangeQuery request, CancellationToken cancellationToken)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists)
                throw new KeyNotFoundException($"User with ID {request.UserId} not found");

            var query = _context.Listings
                .Where(l => l.Price >= request.MinPrice && 
                           l.Price <= request.MaxPrice && 
                           l.Status == Domain.Enums.ListingStatus.Active)
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