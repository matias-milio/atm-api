using ATM.Core.Entities;
using ATM.UseCases.CardHolder.Login;
using System.Reflection;

namespace ATM.Api.Configurations
{
    public static class MediatrConfigs
    {
        public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
        {
            var assemblies = new[]
            {
               Assembly.GetAssembly(typeof(CardHolder)), 
               Assembly.GetAssembly(typeof(LoginCommand)) 
            };
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies!));                
            return services;
        }
    }
}
