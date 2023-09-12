using BLogic.Factories;
using Data;
using Microsoft.EntityFrameworkCore;

namespace ViewModel.Tests
{
    public class InMemoryDbContextFactory : IDbContextFactory
    {
        public string Name { get; set; }

        public InMemoryDbContextFactory(string name)
        {
            Name = name;
        }

        public MainDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();
            optionsBuilder.UseInMemoryDatabase("ICS-Database " + Name);
            return new MainDbContext(optionsBuilder.Options);
        }
    }
}