using BLogic.Repositories;

namespace BLogic.Tests
{
    public class MemberRepositoryTestsFixture
    {
        public MemberRepositoryTestsFixture()
        {
            Factory = new InMemoryDbContextFactory("MemberRepoTests");
            Repository = new MemberRepository(Factory);
        }

        public MemberRepository Repository { get; }
        public InMemoryDbContextFactory Factory { get; }
    }
}