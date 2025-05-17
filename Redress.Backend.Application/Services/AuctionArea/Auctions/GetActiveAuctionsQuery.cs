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
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Application.Common.Models;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.AuctionArea.Auctions
{
    public class GetActiveAuctionsQuery : IRequest<PaginatedList<AuctionDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid UserId { get; set; }
    }

    public class GetActiveAuctionsQueryHandler : IRequestHandler<GetActiveAuctionsQuery, PaginatedList<AuctionDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetActiveAuctionsQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<AuctionDto>> Handle(GetActiveAuctionsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            var query = _context.Auctions
                .Include(a => a.Listing)
                .Include(a => a.Bids)
                .Where(a => a.Listing.IsAuction && 
                           a.Listing.Status == ListingStatus.Active &&
                           a.EndAt > DateTime.UtcNow)
                .OrderBy(a => a.EndAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<AuctionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PaginatedList<AuctionDto>(items, request.Page, request.PageSize, totalCount);
        }
    }
}
