using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using Redress.Backend.Domain.Enums;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Services.ListingArea.Categories
{
    public class GetAllCategoriesBySexQuery : IRequest<List<CategoryDto>>
    {
        public Sex Sex { get; set; }
        public Guid UserId { get; set; }
    }

    public class GetAllCategoriesBySexQueryHandler : IRequestHandler<GetAllCategoriesBySexQuery, List<CategoryDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCategoriesBySexQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> Handle(GetAllCategoriesBySexQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            var categories = await _context.Categories
                .Where(c => c.Sex == request.Sex)
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<CategoryDto>>(categories);
        }
    }
}