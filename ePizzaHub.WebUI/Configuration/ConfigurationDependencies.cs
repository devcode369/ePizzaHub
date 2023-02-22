using ePizzaHub.Services.Implementations;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.WebUI.Helpers;
using ePizzaHub.WebUI.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ePizzaHub.WebUI.Configuration
{
    public class ConfigurationDependencies
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IUserAccessor, UserAccessor>();
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddTransient<IFileHelper, FileHelper>();
        }
    }
}
