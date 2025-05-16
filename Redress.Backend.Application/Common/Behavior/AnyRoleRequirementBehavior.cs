using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Redress.Backend.Application.Common.Behavior
{
    /// <summary>
    /// Поведение для проверки наличия у пользователя одной из требуемых ролей.
    /// Применяется к запросам, реализующим интерфейс IRequireAnyRole.
    /// </summary>
    public class AnyRoleRequirementBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRedressDbContext _context;

        public AnyRoleRequirementBehavior(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Проверяем, реализует ли запрос интерфейс IRequireAnyRole
            if (request is IRequireAnyRole roleRequest)
            {
                // Проверяем существование пользователя
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == roleRequest.UserId, cancellationToken);

                if (user == null)
                    throw new KeyNotFoundException($"Пользователь не найден: {roleRequest.UserId}");

                // Проверяем, есть ли у пользователя одна из требуемых ролей
                if (!roleRequest.RequiredRoles.Contains(user.Role))
                {
                    throw new UnauthorizedAccessException(
                        $"Доступ запрещен: требуется одна из ролей {string.Join(", ", roleRequest.RequiredRoles)}, " +
                        $"но пользователь имеет роль {user.Role}");
                }
            }

            // Продолжаем обработку запроса
            return await next();
        }
    }
} 