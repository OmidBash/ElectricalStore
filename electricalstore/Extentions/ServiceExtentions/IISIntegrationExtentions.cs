using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace electricalstore.Extentions.ServiceExtentions
{
    public static class IISIntegrationExtentions
    {
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }
    }
}