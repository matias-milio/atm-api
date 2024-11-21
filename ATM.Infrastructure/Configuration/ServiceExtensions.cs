using ATM.Core.Contracts;
using ATM.Infrastructure.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ATM.Infrastructure.Configuration
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfigurationManager config)
        {
            services.AddDbContext<AtmDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                options.EnableDetailedErrors();
            });
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped(typeof(IAtmRepository<>), typeof(AtmRepository<>));           
           
            return services;
        }
    }
}
