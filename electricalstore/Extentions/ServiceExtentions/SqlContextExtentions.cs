using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Entities;

namespace electricalstore.Extentions.ServiceExtentions
{
    public static class SqlContextExtentions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder.ConnectionString = config.GetConnectionString("SqlConnection");
            sqlConnectionStringBuilder.UserID = config["UserID"];
            sqlConnectionStringBuilder.Password = config["Password"];

            services.AddDbContext<ElectricalStoreContext>(options =>
            {
                options.UseSqlServer(sqlConnectionStringBuilder.ConnectionString)
                .EnableSensitiveDataLogging();
            });
        }
    }
}