using MediatR;
using Redress.Backend.Contracts.DTOs.CreateDTOs;
using Redress.Backend.Contracts.DTOs.ReadingDTOs;
using Redress.Backend.Contracts.DTOs.UpdateDTOs;
using Redress.Backend.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Enums;
using Redress.Backend.Application.Common.Behavior;

namespace Redress.Backend.Application.Services.ListingArea.Categories
{
    public class CreateCategoryCommand : IRequest<CategoryDto>, IRequireRole
    {
        public CategoryCreateDto Category { get; set; }
        public Guid UserId { get; set; }

        public UserRole RequiredRole => UserRole.Admin;
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IRedressDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            // Check if parent category exists if ParentId is provided
            if (request.Category.ParentId.HasValue)
            {
                var parentExists = await _context.Categories
                    .AnyAsync(c => c.Id == request.Category.ParentId.Value, cancellationToken);

                if (!parentExists)
                    throw new KeyNotFoundException($"Parent category with ID {request.Category.ParentId} not found");
            }

            var category = _mapper.Map<Category>(request.Category);
            await _context.Categories.AddAsync(category, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryDto>(category);
        }
    }
} 