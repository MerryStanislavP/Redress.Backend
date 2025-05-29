using Microsoft.Extensions.DependencyInjection;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Infrastructure.Integration.FileStorage;
using Redress.Backend.Infrastructure.Integration.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Supabase;

namespace Redress.Backend.Infrastructure.Integration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureIntegration(this IServiceCollection services, IConfiguration configuration)
        {
            // Привязываем секцию "Supabase" к опциям
            services.Configure<SupabaseStorageOptions>(configuration.GetSection("Supabase"));

            // Регистрируем Supabase client с инициализацией
            services.AddSingleton(provider =>
            {
                var options = provider.GetRequiredService<IOptions<SupabaseStorageOptions>>().Value;
                
                if (string.IsNullOrEmpty(options.Url) || string.IsNullOrEmpty(options.Key))
                    throw new InvalidOperationException("Supabase configuration is missing. Please add Supabase:Url and Supabase:Key to your configuration.");

                var supabaseOptions = new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true
                };

                var supabaseClient = new Supabase.Client(options.Url, options.Key, supabaseOptions);
                supabaseClient.InitializeAsync().GetAwaiter().GetResult();
                return supabaseClient;
            });

            // Регистрируем сервисы
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserContextService, UserContextService>();

            return services;
        }
    }
}
