using ATM.Infrastructure.Configuration;
using ATM.Infrastructure.Settings;

namespace ATM.Api.Configurations
{
    public static class ServiceConfigs
    {
        public static IServiceCollection AddServiceConfigs(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
            services.AddInfrastructureServices(builder.Configuration);
            services.AddMediatrConfigs();
            services.AddMemoryCache();
            return services;
        }
    }
}
