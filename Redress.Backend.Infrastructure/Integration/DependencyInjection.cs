using Microsoft.Extensions.DependencyInjection;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Infrastructure.Integration.FileStorage;

namespace Redress.Backend.Infrastructure.Integration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFileStorage(this IServiceCollection services, string baseDirectory)
        {
            // Регистрируем FileService с передачей параметра baseDirectory
            services.AddSingleton<IFileService>(provider => new FileService(baseDirectory));
            return services;
        }
    }
}
