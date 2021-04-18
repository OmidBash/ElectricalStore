using Contracts.Repositories;
using Repository;
using Microsoft.Extensions.DependencyInjection;
using Entities.Options;
using Microsoft.Extensions.Configuration;

namespace electricalstore.Extentions.ServiceExtentions
{
    public static class DIExtentions
    {
        public static void ConfigureDI(this IServiceCollection services)
        {


            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            
            services.AddScoped<IIdentityRepositoryWrapper, IdentityRepositoryWrapper>();
        }
    }
}