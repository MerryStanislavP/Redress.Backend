using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Redress.Backend.Contracts.DTOs.ReadingDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.UserArea.Feedback
{
    public class GetFeedbacksForProfileQuery : IRequest<IEnumerable<FeedbackDto>>
    {
        public Guid ProfileId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetFeedbacksForProfileQueryHandler : IRequestHandler<GetFeedbacksForProfileQuery, IEnumerable<FeedbackDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetFeedbacksForProfileQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FeedbackDto>> Handle(GetFeedbacksForProfileQuery request, CancellationToken cancellationToken)
        {
            var feedbacks = await _context.Deals
                .Include(d => d.Feedback)
                .Where(d => d.ProfileId == request.ProfileId && d.Feedback != null)
                .Select(d => d.Feedback)
                .OrderByDescending(f => f.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
        }
    }
}
