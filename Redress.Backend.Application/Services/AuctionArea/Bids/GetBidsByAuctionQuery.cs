using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using AutoMapper;
using Redress.Backend.Application.Common.Models;

namespace Redress.Backend.Application.Services.AuctionArea.Bids
{
    public class GetBidsByAuctionQuery : IRequest<PaginatedList<BidDetailsDto>>
    {
        public Guid AuctionId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetBidsByAuctionQueryHandler : IRequestHandler<GetBidsByAuctionQuery, PaginatedList<BidDetailsDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetBidsByAuctionQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<BidDetailsDto>> Handle(GetBidsByAuctionQuery request, CancellationToken cancellationToken)
        {
            // Verify that auction exists
            var auctionExists = await _context.Auctions
                .AnyAsync(a => a.Id == request.AuctionId, cancellationToken);

            if (!auctionExists)
                throw new KeyNotFoundException($"Auction with ID {request.AuctionId} not found");

            var query = _context.Bids
                .Include(b => b.Profile)
                .Include(b => b.Auction)
                .Where(b => b.AuctionId == request.AuctionId)
                .OrderByDescending(b => b.Amount)
                .ThenByDescending(b => b.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<BidDetailsDto>>(items);

            return new PaginatedList<BidDetailsDto>(
                dtos,
                totalCount,
                request.Page,
                request.PageSize
            );
        }
    }
} 