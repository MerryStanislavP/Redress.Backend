using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.UserArea.Feedback
{
    public class GetAllFeedbacksQuery : IRequest<PaginatedList<FeedbackDetailsDto>>, IRequireAnyRole
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid UserId { get; set; }

        public UserRole[] RequiredRoles => new[] { UserRole.Admin, UserRole.Moderator };
    }

    public class GetAllFeedbacksQueryHandler : IRequestHandler<GetAllFeedbacksQuery, PaginatedList<FeedbackDetailsDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetAllFeedbacksQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<FeedbackDetailsDto>> Handle(GetAllFeedbacksQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Feedbacks
                .Include(f => f.Deal)
                .ThenInclude(d => d.Profile)
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