using System;
using BLogic.Models;
using ViewModel.ViewModels;
using Xunit;

namespace ViewModel.Tests
{
    

    public class TeamViewModelTests : IClassFixture<TeamViewModelTestsFixture>
    {
        private readonly TeamViewModelTestsFixture _fixture;

        public TeamViewModelTests(TeamViewModelTestsFixture fixture)
        {
            _fixture = fixture;
        }
        /*
        private readonly MemberDetailModel _adminMember = new MemberDetailModel
        {
            Email = "ICSadmin@gmail.com",
            Id = Guid.NewGuid(),
            Address = "Czech Republic, Brno",
            FirstName = "Admin",
            IsAdmin = true,
            LastActionDate = DateTime.Now,
            LastName = "Admin's Surname",
            Nickname = "Test admin",
            Password = "root"
        };

        [Fact]
        public void CreateNewViewModel_WithAdminUser_ActiveUserMatches()
        {
            _fixture.ViewModel = new TeamViewModel(_adminMember, _fixture.DbContextFactory);
            Assert.Equal(_fixture.ViewModel.ActiveMember, _adminMember);
        }*/
    }
}