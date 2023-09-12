using BLogic.Repositories;

namespace BLogic.Tests
{
    public class PostRepositoryTestsFixture
    {
        public PostRepositoryTestsFixture()
        {
            Factory = new InMemoryDbContextFactory("PostRepoTests");
            Repository = new PostRepository(Factory);
        }

        public PostRepository Repository { get; }
        public InMemoryDbContextFactory Factory { get; }
    }
}
