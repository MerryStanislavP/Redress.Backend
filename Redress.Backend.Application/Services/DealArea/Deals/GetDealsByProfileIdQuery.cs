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

namespace Redress.Backend.Application.Services.DealArea.Deals
{
    public class GetDealsByProfileIdQuery : IRequest<IEnumerable<DealDto>>
    {
        public Guid ProfileId { get; set; }
    }

    public class GetDealsByProfileIdQueryHandler : IRequestHandler<GetDealsByProfileIdQuery, IEnumerable<DealDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetDealsByProfileIdQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DealDto>> Handle(GetDealsByProfileIdQuery request, CancellationToken cancellationToken)
        {
            var deals = await _context.Deals
                .Include(d => d.Listing)
                .Include(d => d.Profile)
                .Where(d => d.ProfileId == request.ProfileId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<DealDto>>(deals);
        }
    }
}
