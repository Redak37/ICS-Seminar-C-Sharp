using ViewModel.ViewModels;

namespace ViewModel.Tests
{
    public class TeamViewModelTestsFixture
    {
        public TeamViewModelTestsFixture()
        {
            DbContextFactory = new InMemoryDbContextFactory("TeamVMTests");
        }
        public TeamViewModel ViewModel { get; set; }
        public InMemoryDbContextFactory DbContextFactory { get; }
    }
}