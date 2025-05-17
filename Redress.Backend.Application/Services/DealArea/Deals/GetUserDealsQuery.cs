using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Application.Common.Models;
using AutoMapper.QueryableExtensions;

namespace Redress.Backend.Application.Services.DealArea.Deals
{
    public class GetUserDealsQuery : IRequest<PaginatedList<DealDetailsDto>>
    {
        public Guid UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetUserDealsQueryHandler : IRequestHandler<GetUserDealsQuery, PaginatedList<DealDetailsDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetUserDealsQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<DealDetailsDto>> Handle(GetUserDealsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            var query = _context.Deals
                .Include(d => d.Listing)
                .Include(d => d.Profile)
                .Include(d => d.Feedback)
                .Where(d => d.Profile.UserId == request.UserId || d.Listing.Profile.UserId == request.UserId)
                .OrderByDescending(d => d.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<DealDetailsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PaginatedList<DealDetailsDto>(items, request.Page, request.PageSize, totalCount);
        }
    }
} 