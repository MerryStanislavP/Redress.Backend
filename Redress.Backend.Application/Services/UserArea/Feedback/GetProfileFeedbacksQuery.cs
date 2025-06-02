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

namespace Redress.Backend.Application.Services.UserArea.Feedback
{
    public class GetProfileFeedbacksQuery : IRequest<PaginatedList<FeedbackDetailsDto>>
    {
        public Guid ProfileId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid UserId { get; set; }
    }

    public class GetProfileFeedbacksQueryHandler : IRequestHandler<GetProfileFeedbacksQuery, PaginatedList<FeedbackDetailsDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetProfileFeedbacksQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<FeedbackDetailsDto>> Handle(GetProfileFeedbacksQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            var query = _context.Feedbacks
                .Include(f => f.Deal)
                    .ThenInclude(d => d.Listing)
                        .ThenInclude(l => l.ProfileId)
                .Where(f => f.Deal.Listing.ProfileId == request.ProfileId)
                .OrderByDescending(f => f.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<FeedbackDetailsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PaginatedList<FeedbackDetailsDto>(items, request.Page, request.PageSize, totalCount);
        }
    }
} 