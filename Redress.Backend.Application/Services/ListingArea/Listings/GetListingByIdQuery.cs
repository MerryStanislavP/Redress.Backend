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
    public class GetListingByIdQuery : IRequest<ListingDto>
    {
        public Guid Id { get; set; }
    }

    public class GetListingByIdQueryHandler : IRequestHandler<GetListingByIdQuery, ListingDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetListingByIdQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListingDto> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
        {
            var listing = await _context.Listings
                .Include(l => l.Category)
                .Include(l => l.Profile)
                .Include(l => l.ListingImages)
                .Include(l => l.Auction)
                .Include(l => l.Deal)
                .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {request.Id} not found");

            return _mapper.Map<ListingDto>(listing);
        }
    }
}
