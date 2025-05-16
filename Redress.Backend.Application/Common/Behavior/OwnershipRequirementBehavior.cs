using Redress.Backend.Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Redress.Backend.Application.Common.Behavior
{
    /// <summary>
    /// Поведение для проверки владения сущностью.
    /// Применяется к запросам, реализующим интерфейс IOwnershipCheck.
    /// </summary>
    public class OwnershipRequirementBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRedressDbContext _context;

        public OwnershipRequirementBehavior(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Проверяем, реализует ли запрос интерфейс IOwnershipCheck
            if (request is IOwnershipCheck ownershipCheck)
            {
                // Проверяем существование пользователя
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == ownershipCheck.UserId, cancellationToken);

                if (user == null)
                    throw new KeyNotFoundException($"Пользователь не найден: {ownershipCheck.UserId}");

                // Проверяем владение
                bool isOwner = await ownershipCheck.CheckOwnershipAsync(_context, cancellationToken);

                // Если пользователь не владелец, проверка завершается с ошибкой
                // Проверка ролей будет выполнена отдельным поведением RoleRequirementBehavior
                if (!isOwner)
                {
                    throw new UnauthorizedAccessException("Доступ запрещен: вы не являетесь владельцем этого ресурса");
                }
            }

            // Продолжаем обработку запроса
            return await next();
        }
    }
}
