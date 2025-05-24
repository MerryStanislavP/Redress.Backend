using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Domain.Entities;
using Redress.Backend.Domain.Enums;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Redress.Backend.Application.Services.ListingArea.Categories
{
    public class GetAllCategoriesBySexQuery : IRequest<List<CategoryTreeDto>>
    {
        public Sex Sex { get; set; }
        public Guid UserId { get; set; }
    }

    public class GetAllCategoriesBySexQueryHandler : IRequestHandler<GetAllCategoriesBySexQuery, List<CategoryTreeDto>>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCategoriesBySexQueryHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoryTreeDto>> Handle(GetAllCategoriesBySexQuery request, CancellationToken cancellationToken)
        {
            // Получаем все категории для указанного пола
            var categories = await _context.Categories
                .Where(c => c.Sex == request.Sex)
                .ToListAsync(cancellationToken);

            // Получаем корневые категории (те, у которых нет родителя)
            var rootCategories = categories
                .Where(c => c.ParentId == null)
                .ToList();

            // Строим дерево для каждой корневой категории
            var categoryTree = rootCategories
                .Select(category => BuildCategoryTree(category, categories))
                .ToList();

            return categoryTree;
        }

        private CategoryTreeDto BuildCategoryTree(Category category, List<Category> allCategories)
        {
            var categoryDto = _mapper.Map<CategoryTreeDto>(category);

            // Рекурсивно строим дерево для дочерних категорий
            var childCategories = allCategories
                .Where(c => c.ParentId == category.Id)
                .ToList();

            categoryDto.Children = childCategories
                .Select(child => BuildCategoryTree(child, allCategories))
                .ToList();

            return categoryDto;
        }
    }
}