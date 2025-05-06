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

namespace Redress.Backend.Application.Services.ListingArea.Listings
{
    public class GetListingsByCategoryQuery : IRequest<IEnumerable<ListingDto>>
    {
        public Guid CategoryId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetListingsByCategoryQueryHandler : IRequestHandler<GetListingsByCategoryQuery, IEnumerable<ListingDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetListingsByCategoryQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ListingDto>> Handle(GetListingsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var listings = await _context.Listings
                .Include(l => l.Category)
                .Include(l => l.Profile)
                .Include(l => l.ListingImages)
                .Where(l => l.CategoryId == request.CategoryId)
                .OrderByDescending(l => l.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<ListingDto>>(listings);
        }
    }
}
