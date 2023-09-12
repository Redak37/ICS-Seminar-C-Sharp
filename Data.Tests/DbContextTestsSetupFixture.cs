using System;
using Microsoft.EntityFrameworkCore;

namespace Data.Tests
{
    public class DbContextTestsSetupFixture : IDisposable
    {
        public MainDbContext TestDbContext { get; set; } = new MainDbContext(
            new DbContextOptionsBuilder<MainDbContext>().UseInMemoryDatabase("ICS-Database-DATA").Options
            );

        public void Dispose()
        {
            TestDbContext?.Dispose();
        }
    }
}
