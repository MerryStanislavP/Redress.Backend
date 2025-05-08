using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Redress.Backend.Contracts.DTOs.ReadingDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Application.Common.Models;

namespace Redress.Backend.Application.Services.DealArea.Deals
{
    public class GetDealsByProfileIdQuery : IRequest<PaginatedList<DealDto>>
    {
        public Guid ProfileId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetDealsByProfileIdQueryHandler : IRequestHandler<GetDealsByProfileIdQuery, PaginatedList<DealDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetDealsByProfileIdQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<DealDto>> Handle(GetDealsByProfileIdQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Deals
                .Where(d => d.ProfileId == request.ProfileId)
                .OrderByDescending(d => d.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<DealDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PaginatedList<DealDto>(items, request.Page, request.PageSize, totalCount);
        }
    }
}
