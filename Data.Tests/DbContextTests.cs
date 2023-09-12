using System.Linq;
using Xunit;

namespace Data.Tests
{
    public class DbContextTests : IClassFixture<DbContextTestsSetupFixture>
    {
        public DbContextTests(DbContextTestsSetupFixture testContext)
        {
            _testContext = testContext;
        }

        private readonly DbContextTestsSetupFixture _testContext;

        [Fact]
        public void AddMemberTest()
        {
            var testMember = new Member
            {
                
                FirstName = "Ievan",
                LastName = "von Testovitzch",
                Email = "Ievan.t@test.com",
                IsAdmin = false
            };

            _testContext.TestDbContext.Members.Add(testMember);
            _testContext.TestDbContext.SaveChanges();
            
            var retrievedMember = _testContext.TestDbContext.Members.First(member => member.Id == testMember.Id);
            Assert.Equal(testMember, retrievedMember, Member.MemberComparer);
            Assert.Equal(testMember.Email, retrievedMember.Email);
        }
        
        [Fact]
        public void AddTeamTest()
        {
            var testTeam = new Team
            {
                Name = "Test Team Tumber Tone"
            };

            _testContext.TestDbContext.Teams.Add(testTeam);
            _testContext.TestDbContext.SaveChanges();

            var retrievedTeam = _testContext.TestDbContext.Teams.First(team => team.Id == testTeam.Id);
            Assert.Equal(testTeam, retrievedTeam, Team.TeamComparer);
            Assert.Equal(testTeam.Name, retrievedTeam.Name);
        }

        [Fact]
        public void AddPostTest()
        {
            var testPost = new Post
            {
                Title = "The first ever super duper post.",
                Text = "Whatever content of this awesome post."
            };

            _testContext.TestDbContext.Posts.Add(testPost);
            _testContext.TestDbContext.SaveChanges();

            var retrievedPost = _testContext.TestDbContext.Posts.First(post => post.Id == testPost.Id);
            Assert.Equal(testPost, retrievedPost, Post.PostComparer);
            Assert.Equal(testPost.Title, retrievedPost.Title);
        }
        
        [Fact]
        public void AddCommentTest()
        {
            var testComment = new Comment
            {
                Text = "That post from above ain't that great."
            };

            _testContext.TestDbContext.Comments.Add(testComment);
            _testContext.TestDbContext.SaveChanges();

            var retrievedComment = _testContext.TestDbContext.Comments.First(comment => comment.Id == testComment.Id);
            Assert.Equal(testComment, retrievedComment, Comment.CommentComparer);
            Assert.Equal(testComment.Text, retrievedComment.Text);
        }
    }
}
