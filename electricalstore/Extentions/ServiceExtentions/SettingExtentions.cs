using Entities.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace electricalstore.Extentions.ServiceExtentions
{
    public static class SettingExtentions
    {
        public static void ConfigSettings(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtSettings>(config.GetSection(JwtSettings.SectionName));
        }
    }
}