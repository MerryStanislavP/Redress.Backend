using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Common.Models;
using AutoMapper.QueryableExtensions;

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class GetListingsByProfileQuery : IRequest<PaginatedList<ListingDto>>
    {
        public Guid ProfileId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid UserId { get; set; }
    }

    public class GetListingsByProfileQueryHandler : IRequestHandler<GetListingsByProfileQuery, PaginatedList<ListingDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetListingsByProfileQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ListingDto>> Handle(GetListingsByProfileQuery request, CancellationToken cancellationToken)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists)
                throw new KeyNotFoundException($"User with ID {request.UserId} not found");

            var query = _context.Listings
                .Where(l => l.ProfileId == request.ProfileId)
                .Include(l => l.ListingImages)
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