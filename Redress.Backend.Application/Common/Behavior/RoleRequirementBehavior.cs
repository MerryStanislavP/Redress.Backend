using MediatR;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Application.Common.Behavior
{

    public class RoleRequirementBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRedressDbContext _context;

        public RoleRequirementBehavior(IRedressDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IRequireRole roleRequest)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == roleRequest.UserId, cancellationToken)
                    ?? throw new KeyNotFoundException($"User with ID {roleRequest.UserId} not found");

                if (user.Role != roleRequest.RequiredRole)
                    throw new UnauthorizedAccessException($"Access denied: required role is {roleRequest.RequiredRole}, but user has role {user.Role}");
            }

            return await next();
        }
    }
}
