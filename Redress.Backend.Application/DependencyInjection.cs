using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using Redress.Backend.Application.Common.Behavior;

namespace Redress.Backend.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddValidatorsFromAssemblies(new[] {Assembly.GetExecutingAssembly()});
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RoleRequirementBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AnyRoleRequirementBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(OwnershipRequirementBehavior<,>));
            return services;
        }
    }
}
