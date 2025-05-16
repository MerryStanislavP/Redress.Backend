using System;
using System.Threading;
using System.Threading.Tasks;

namespace Redress.Backend.Application.Interfaces
{
    /// <summary>
    /// Интерфейс для проверки владения сущностью.
    /// Запросы, реализующие этот интерфейс, будут проверяться на владение сущностью.
    /// </summary>
    public interface IOwnershipCheck
    {
        /// <summary>
        /// ID пользователя, выполняющего запрос
        /// </summary>
        Guid UserId { get; }
        
        /// <summary>
        /// Проверяет, имеет ли пользователь право на доступ к сущности
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>True, если пользователь имеет право на доступ; иначе false</returns>
        Task<bool> CheckOwnershipAsync(IRedressDbContext context, CancellationToken cancellationToken);
    }
}
