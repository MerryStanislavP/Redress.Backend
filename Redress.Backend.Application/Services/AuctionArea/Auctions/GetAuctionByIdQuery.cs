using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.AuctionArea.Auctions
{
    public class GetAuctionByIdQuery : IRequest<AuctionDetailsDto>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }

    public class GetAuctionByIdQueryHandler : IRequestHandler<GetAuctionByIdQuery, AuctionDetailsDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetAuctionByIdQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AuctionDetailsDto> Handle(GetAuctionByIdQuery request, CancellationToken cancellationToken)
        {
            var auction = await _context.Auctions
                .Include(a => a.Listing)
                .Include(a => a.Bids)
                .ThenInclude(b => b.Profile)
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (auction == null)
                throw new KeyNotFoundException($"Auction with ID {request.Id} not found");

            var auctionDto = _mapper.Map<AuctionDetailsDto>(auction);
            
            // Calculate current price and bid count
            auctionDto.CurrentPrice = auction.Bids.Any() 
                ? auction.Bids.Max(b => b.Amount) 
                : auction.StartPrice;
            auctionDto.BidCount = auction.Bids.Count;
            auctionDto.IsActive = auction.EndAt > DateTime.UtcNow;

            return auctionDto;
        }
    }
} 