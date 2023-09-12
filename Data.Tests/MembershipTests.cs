using System.Drawing;
using System.Linq;
using Xunit;

namespace Data.Tests
{
    public class MembershipTests : IClassFixture<DbContextTestsSetupFixture>
    {
        public MembershipTests(DbContextTestsSetupFixture testMembership)
        {
            _testMembership = testMembership;
        }

        private readonly DbContextTestsSetupFixture _testMembership;

        [Fact]
        public void MembershipsTest()
        {
            var testMemberAdmin = new Member
            {
                FirstName = "John",
                LastName = "Doe",
                Nickname = "JDoe84",
                Email = "john.doe@gmail.com",
                IsAdmin = true,
                Address = "3020 5th Avenue, New York, NY"
            };

            _testMembership.TestDbContext.Members.Add(testMemberAdmin);
            _testMembership.TestDbContext.SaveChanges();

            var retrievedMember =
                _testMembership.TestDbContext.Members.First(member => member.Id == testMemberAdmin.Id);
            Assert.Equal(testMemberAdmin, retrievedMember, Member.MemberComparer);
            Assert.Equal(testMemberAdmin.FirstName, retrievedMember.FirstName);
            Assert.Equal(testMemberAdmin.LastName, retrievedMember.LastName);
            Assert.Equal(testMemberAdmin.Nickname, retrievedMember.Nickname);
            Assert.Equal(testMemberAdmin.Email, retrievedMember.Email);
            Assert.Equal(testMemberAdmin.IsAdmin, retrievedMember.IsAdmin);
            Assert.Equal(testMemberAdmin.Address, retrievedMember.Address);

            var testMemberNotAdmin = new Member
            {
                FirstName = "George",
                LastName = "Smith",
                Nickname = "CodeMasterGeorge",
                Email = "gsmith@yahoo.com",
                IsAdmin = false,
                Address = "4343 Baker Avenue, Farmers Branch, TX"
            };

            _testMembership.TestDbContext.Members.Add(testMemberNotAdmin);
            _testMembership.TestDbContext.SaveChanges();

            retrievedMember = _testMembership.TestDbContext.Members.Last(member => member.Id == testMemberNotAdmin.Id);
            Assert.Equal(testMemberNotAdmin, retrievedMember, Member.MemberComparer);
            Assert.Equal(testMemberNotAdmin.FirstName, retrievedMember.FirstName);
            Assert.Equal(testMemberNotAdmin.LastName, retrievedMember.LastName);
            Assert.Equal(testMemberNotAdmin.Nickname, retrievedMember.Nickname);
            Assert.Equal(testMemberNotAdmin.Email, retrievedMember.Email);
            Assert.Equal(testMemberNotAdmin.IsAdmin, retrievedMember.IsAdmin);
            Assert.Equal(testMemberNotAdmin.Address, retrievedMember.Address);

            var testTeam = new Team
            {
                Name = "C# Experts",
                RGB = Color.Blue
            };

            _testMembership.TestDbContext.Teams.Add(testTeam);
            _testMembership.TestDbContext.SaveChanges();

            var retrievedTeam = _testMembership.TestDbContext.Teams.First(team => team.Id == testTeam.Id);
            Assert.Equal(testTeam, retrievedTeam, Team.TeamComparer);
            Assert.Equal(testTeam.Name, retrievedTeam.Name);
            Assert.Equal(testTeam.RGB, retrievedTeam.RGB);

            var testMembershipAdmin = new Membership
            {
                IsFounder = true,
                IsAdmin = true,
                Member = testMemberAdmin,
                Team = testTeam
            };

            _testMembership.TestDbContext.Memberships.Add(testMembershipAdmin);
            _testMembership.TestDbContext.SaveChanges();

            var retrievedMembership =
                _testMembership.TestDbContext.Memberships.First(membership => membership.Id == testMembershipAdmin.Id);
            Assert.Equal(testMembershipAdmin, retrievedMembership, Membership.MembershipComparer);
            Assert.Equal(testMembershipAdmin.IsFounder, retrievedMembership.IsFounder);
            Assert.Equal(testMembershipAdmin.IsAdmin, retrievedMembership.IsAdmin);
            Assert.Equal(testMembershipAdmin.Member, retrievedMembership.Member);
            Assert.Equal(testMembershipAdmin.Team, retrievedMembership.Team);

            var testMembershipNotAdmin = new Membership
            {
                IsFounder = false,
                IsAdmin = false,
                Member = testMemberNotAdmin,
                Team = testTeam
            };

            _testMembership.TestDbContext.Memberships.Add(testMembershipNotAdmin);
            _testMembership.TestDbContext.SaveChanges();

            retrievedMembership =
                _testMembership.TestDbContext.Memberships.Last(membership =>
                    membership.Id == testMembershipNotAdmin.Id);
            Assert.Equal(testMembershipNotAdmin, retrievedMembership, Membership.MembershipComparer);
            Assert.Equal(testMembershipNotAdmin.IsFounder, retrievedMembership.IsFounder);
            Assert.Equal(testMembershipNotAdmin.IsAdmin, retrievedMembership.IsAdmin);
            Assert.Equal(testMembershipNotAdmin.Member, retrievedMembership.Member);
            Assert.Equal(testMembershipNotAdmin.Team, retrievedMembership.Team);
        }
    }
}