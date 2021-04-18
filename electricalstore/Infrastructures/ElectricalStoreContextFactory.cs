using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace electricalstore.Infrastructures
{
    public class ElectricalStoreContextFactory : IDesignTimeDbContextFactory<ElectricalStoreContext>
    {
        public ElectricalStoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ElectricalStoreContext>();
            optionsBuilder.UseSqlServer(
                "Data Source=localhost;Initial Catalog=ElectricalStore;User ID=SA;Password=sqlSApa3$;",
                b => b.MigrationsAssembly("electricalstore"));

            return new ElectricalStoreContext(optionsBuilder.Options);
        }
    }
}