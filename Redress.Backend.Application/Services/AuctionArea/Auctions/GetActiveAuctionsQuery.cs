using MediatR;
using Redress.Backend.Contracts.DTOs.ReadingDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.AuctionArea.Auctions
{
    public class GetActiveAuctionsQuery : IRequest<IEnumerable<AuctionDto>> {}

    public class GetActiveAuctionsQueryHandler : IRequestHandler<GetActiveAuctionsQuery, IEnumerable<AuctionDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetActiveAuctionsQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuctionDto>> Handle(GetActiveAuctionsQuery request, CancellationToken cancellationToken)
        {
            var activeAuctions = await _context.Auctions
                .Include(a => a.Listing)
                .Where(a => a.EndAt == null || a.EndAt > DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<AuctionDto>>(activeAuctions);
        }
    }
}
