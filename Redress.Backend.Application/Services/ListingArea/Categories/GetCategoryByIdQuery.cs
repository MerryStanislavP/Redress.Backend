using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.ListingArea.Categories
{
    public class GetCategoryByIdQuery : IRequest<CategoryTreeDto>
    {
        public Guid ListingId { get; set; }
        public Guid UserId { get; set; }
    }

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryTreeDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryTreeDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            var listing = await _context.Listings
                .Include(l => l.Category)
                .ThenInclude(c => c.Parent)
                .FirstOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {request.ListingId} not found");

            if (listing.Category == null)
                throw new KeyNotFoundException($"Category not found for listing {request.ListingId}");

            return _mapper.Map<CategoryTreeDto>(listing.Category);
        }
    }
} 