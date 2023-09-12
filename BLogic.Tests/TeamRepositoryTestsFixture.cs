using BLogic.Repositories;

namespace BLogic.Tests
{
    public class TeamRepositoryTestsFixture
    {
        public TeamRepositoryTestsFixture()
        {
            Factory = new InMemoryDbContextFactory("TeamRepoTests");
            Repository = new TeamRepository(Factory);
        }

        public TeamRepository Repository { get; }
        public InMemoryDbContextFactory Factory { get; }
    }
}
