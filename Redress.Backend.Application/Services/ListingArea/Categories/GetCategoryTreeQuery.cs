using MediatR;
using Redress.Backend.Contracts.DTOs.ReadingDTO;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Redress.Backend.Application.Services.ListingArea.Categories
{
    public class GetCategoryTreeQuery : IRequest<IEnumerable<CategoryTreeDto>>
    {
    }

    public class GetCategoryTreeQueryHandler : IRequestHandler<GetCategoryTreeQuery, IEnumerable<CategoryTreeDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetCategoryTreeQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryTreeDto>> Handle(GetCategoryTreeQuery request, CancellationToken cancellationToken)
        {
            // Get all categories with their children
            var categories = await _context.Categories
                .Include(c => c.Children)
                .ToListAsync(cancellationToken);

            // Get root categories (those without parents)
            var rootCategories = categories.Where(c => c.ParentId == null).ToList();

            // Build tree structure
            var categoryTree = rootCategories.Select(category => BuildCategoryTree(category, categories));

            return categoryTree;
        }

        private CategoryTreeDto BuildCategoryTree(Category category, List<Category> allCategories)
        {
            var categoryDto = _mapper.Map<CategoryTreeDto>(category);

            // Recursively build tree for children
            var childCategories = allCategories.Where(c => c.ParentId == category.Id).ToList();
            categoryDto.Children = childCategories.Select(child => BuildCategoryTree(child, allCategories)).ToList();

            return categoryDto;
        }
    }
}
