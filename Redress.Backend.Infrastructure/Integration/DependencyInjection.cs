using Microsoft.Extensions.DependencyInjection;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Infrastructure.Integration.FileStorage;
using Redress.Backend.Infrastructure.Integration.Authentication;

namespace Redress.Backend.Infrastructure.Integration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureIntegration(this IServiceCollection services, string baseDirectory)
        {
            // Регистрируем FileService с передачей параметра baseDirectory
            services.AddSingleton<IFileService>(provider => new FileService(baseDirectory));

            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IUserContextService, UserContextService>();

            return services;
        }
    }
}
