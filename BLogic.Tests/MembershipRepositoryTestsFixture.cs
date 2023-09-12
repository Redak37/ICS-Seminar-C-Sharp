using BLogic.Repositories;

namespace BLogic.Tests
{
    public class MembershipRepositoryTestsFixture
    {
        public MembershipRepositoryTestsFixture()
        {
            Factory = new InMemoryDbContextFactory("MembershipRepoTests");
            Repository = new MembershipRepository(Factory);
            MemberRepository = new MemberRepository(Factory);
            TeamRepository = new TeamRepository(Factory);
        }

        public MembershipRepository Repository { get; }
        public MemberRepository MemberRepository { get; }
        public TeamRepository TeamRepository { get; }
        public InMemoryDbContextFactory Factory { get; }
    }
}