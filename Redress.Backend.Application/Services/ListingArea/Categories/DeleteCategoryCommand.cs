using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Services.ListingArea.Categories
{
    public class DeleteCategoryCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // ID пользователя, выполняющего удаление
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IRedressDbContext _context;

        public DeleteCategoryCommandHandler(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            // Получаем пользователя для проверки прав
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            // Проверяем, что пользователь - администратор
            if (user.Role != UserRole.Admin)
                throw new UnauthorizedAccessException("Only administrators can delete categories");

            var category = await _context.Categories
                .Include(c => c.Children)
                .Include(c => c.Listings)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
                throw new KeyNotFoundException($"Category with ID {request.Id} not found");

            // Check if category has any listings
            if (category.Listings.Any())
                throw new InvalidOperationException("Cannot delete category that has listings");

            // Check if category has any children
            if (category.Children.Any())
                throw new InvalidOperationException("Cannot delete category that has subcategories");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
} 