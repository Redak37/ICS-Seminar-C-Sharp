using Data;
using Microsoft.EntityFrameworkCore;

namespace BLogic.Factories
{
    public class DbContextFactory : IDbContextFactory
    {
        public MainDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();
            optionsBuilder.UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = ICSProjekt; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
            return new MainDbContext(optionsBuilder.Options);
        }
    }
}
