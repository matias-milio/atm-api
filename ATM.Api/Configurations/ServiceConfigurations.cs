using ATM.Infrastructure.Configuration;
using ATM.Infrastructure.Settings;

namespace ATM.Api.Configurations
{
    public static class ServiceConfigs
    {
        public static IServiceCollection AddServiceConfigs(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddAuthentication();
            services.AddApiVersioning();
            services.AddAuthenticationConfigs(builder.Configuration);
            services.AddInfrastructureServices(builder.Configuration);
            services.AddMediatrConfigs();
            services.AddSwaggerConfigs();
            services.AddMemoryCache();
            services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
            return services;
        }
    }
}
