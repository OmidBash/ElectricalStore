using Microsoft.Extensions.DependencyInjection;

namespace electricalstore.Extentions.ServiceExtentions
{
    public static class ControllerExtentions
    {
        public static void ConfigController(this IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    { options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; }
                );
        }
    }
}