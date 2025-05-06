using MediatR;
using Redress.Backend.Contracts.DTOs.ReadingDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.ListingArea.Favorites
{
    public class GetUserFavoritesQuery : IRequest<IEnumerable<ListingDto>>
    {
        public Guid ProfileId { get; set; }
    }

    public class GetUserFavoritesQueryHandler : IRequestHandler<GetUserFavoritesQuery, IEnumerable<ListingDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetUserFavoritesQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ListingDto>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
        {
            var favorites = await _context.Favorites
                .Include(f => f.Listing)
                .Where(f => f.ProfileId == request.ProfileId)
                .Select(f => f.Listing)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<ListingDto>>(favorites);
        }
    }
}
