using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using AutoMapper;
using Redress.Backend.Domain.Enums;
using Redress.Backend.Application.Common.Models;

namespace Redress.Backend.Application.Services.AuctionArea.Bids
{
    public class GetBidsByProfileQuery : IRequest<PaginatedList<BidDetailsDto>>, IOwnershipCheck
    {
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public async Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == UserId, cancellationToken);

            if (user == null)
                return false;

            // Admin can view any profile's bids
            if (user.Role == UserRole.Admin)
                return true;

            // Get the profile
            var profile = await context.Profiles
                .FirstOrDefaultAsync(p => p.Id == ProfileId, cancellationToken);

            if (profile == null)
                return false;

            // User can only view their own profile's bids
            return profile.UserId == UserId;
        }
    }

    public class GetBidsByProfileQueryHandler : IRequestHandler<GetBidsByProfileQuery, PaginatedList<BidDetailsDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetBidsByProfileQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<BidDetailsDto>> Handle(GetBidsByProfileQuery request, CancellationToken cancellationToken)
        {
            // Verify that profile exists
            var profileExists = await _context.Profiles
                .AnyAsync(p => p.Id == request.ProfileId, cancellationToken);

            if (!profileExists)
                throw new KeyNotFoundException($"Profile with ID {request.ProfileId} not found");

            var query = _context.Bids
                .Include(b => b.Profile)
                .Include(b => b.Auction)
                .Where(b => b.ProfileId == request.ProfileId)
                .OrderByDescending(b => b.CreatedAt);

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