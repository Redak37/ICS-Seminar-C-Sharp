using BLogic.Repositories;

namespace BLogic.Tests
{
    public class CommentRepositoryTestsFixture
    {
        public CommentRepositoryTestsFixture()
        {
            Factory = new InMemoryDbContextFactory("CommentRepoTests");
            Repository = new CommentRepository(Factory);
        }

        public CommentRepository Repository { get; }
        public InMemoryDbContextFactory Factory { get; }
    }
}