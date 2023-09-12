using Data;

namespace BLogic.Factories
{
    public interface IDbContextFactory
    {
        MainDbContext CreateDbContext();
    }
}