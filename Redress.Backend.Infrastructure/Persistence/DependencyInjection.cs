using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["DbConnection"];
            services.AddDbContext<RedressDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            services.AddScoped<IRedressDbContext>(provider =>
                provider.GetService<RedressDbContext>());
            return services;
        }
    }
}
