﻿using System;
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

namespace Redress.Backend.Application.Services.UserArea.Feedback
{
    public class GetFeedbacksForProfileQuery : IRequest<PaginatedList<FeedbackDto>>
    {
        public Guid ProfileId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetFeedbacksForProfileQueryHandler : IRequestHandler<GetFeedbacksForProfileQuery, PaginatedList<FeedbackDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetFeedbacksForProfileQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<FeedbackDto>> Handle(GetFeedbacksForProfileQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Deals
                .Where(d => d.ProfileId == request.ProfileId && d.Feedback != null)
                .Select(d => d.Feedback)
                .OrderByDescending(f => f.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<FeedbackDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PaginatedList<FeedbackDto>(items, request.Page, request.PageSize, totalCount);
        }
    }
}
