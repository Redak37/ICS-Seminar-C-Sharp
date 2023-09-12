using System;
using System.Drawing;
using BLogic.Models;
using Xunit;

namespace BLogic.Tests
{
    public class MembershipRepositoryTests : IClassFixture<MembershipRepositoryTestsFixture>
    {
        private readonly MembershipRepositoryTestsFixture _fixture;

        public MembershipRepositoryTests(MembershipRepositoryTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new MembershipModel
            {
                IsAdmin = true,
                Id = Guid.NewGuid(),
                IsFounder = true,
                Member = new MemberDetailModel
                {
                    FirstName = "Abraham",
                    LastName = "Lincoln",
                    Nickname = "Strejda",
                    Email = "A.lin1@example.com",
                    Address = "Example Address 3",
                    IsAdmin = false,
                    Id = Guid.NewGuid(),
                    LastActionDate = DateTime.Now
                },
                Team = new TeamDetailModel
                {
                    Name = "Name of some testing Team.",
                    RGB = Color.FromArgb(255, 0, 0),
                    Id = Guid.NewGuid()
                }
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotNull(returnedModel);

            _fixture.Repository.Delete(returnedModel.Id);
            _fixture.TeamRepository.Delete(returnedModel.Team.Id);
            _fixture.MemberRepository.Delete(returnedModel.Member.Id);
        }

        [Fact]
        public void Create_WithNonExistingItemAndEmptySet_DbSetNotEmpty()
        {
            Assert.Empty(_fixture.Factory.CreateDbContext().Memberships);

            var model = new MembershipModel
            {
                IsAdmin = true,
                Id = Guid.NewGuid(),
                IsFounder = true,
                Member = new MemberDetailModel
                {
                    FirstName = "Abraham",
                    LastName = "Lincoln",
                    Nickname = "Strejda",
                    Email = "A.lin2@example.com",
                    Address = "Example Address 3",
                    IsAdmin = false,
                    Id = Guid.NewGuid(),
                    LastActionDate = DateTime.Now
                },
                Team = new TeamDetailModel
                {
                    Name = "Name of some testing Team.",
                    RGB = Color.FromArgb(255, 0, 0),
                    Id = Guid.NewGuid()
                }
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotEmpty(_fixture.Factory.CreateDbContext().Memberships);

            _fixture.Repository.Delete(returnedModel.Id);
            _fixture.TeamRepository.Delete(returnedModel.Team.Id);
            _fixture.MemberRepository.Delete(returnedModel.Member.Id);
        }
        [Fact]
        public void Create_WithNonExistingItem_ModelsMatch()
        {
            var model = new MembershipModel
            {
                IsAdmin = true,
                Id = Guid.NewGuid(),
                IsFounder = true,
                Member = new MemberDetailModel
                {
                    FirstName = "Abraham",
                    LastName = "Lincoln",
                    Nickname = "Strejda",
                    Email = "A.lin3@example.com",
                    Address = "Example Address 3",
                    IsAdmin = false,
                    Id = Guid.NewGuid(),
                    LastActionDate = DateTime.Now
                },
                Team = new TeamDetailModel
                {
                    Name = "Name of some testing Team.",
                    RGB = Color.FromArgb(255, 0, 0),
                    Id = Guid.NewGuid()
                }
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.True(model.IsAdmin == returnedModel.IsAdmin && model.Id == returnedModel.Id);

            _fixture.Repository.Delete(returnedModel.Id);
            _fixture.TeamRepository.Delete(returnedModel.Team.Id);
            _fixture.MemberRepository.Delete(returnedModel.Member.Id);
        }

        [Fact]
        public void DeleteAfterCreating_WithNonExistingItemAndEmptySet_DbSetIsEmpty()
        {
            Assert.Empty(_fixture.Factory.CreateDbContext().Memberships);

            var model = new MembershipModel
            {
                IsAdmin = true,
                Id = Guid.NewGuid(),
                IsFounder = true,
                Member = new MemberDetailModel
                {
                    FirstName = "Abraham",
                    LastName = "Lincoln",
                    Nickname = "Strejda",
                    Email = "A.lin4@example.com",
                    Address = "Example Address 3",
                    IsAdmin = false,
                    Id = Guid.NewGuid(),
                    LastActionDate = DateTime.Now
                },
                Team = new TeamDetailModel
                {
                    Name = "Name of some testing Team.",
                    RGB = Color.FromArgb(255, 0, 0),
                    Id = Guid.NewGuid()
                }
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotEmpty(_fixture.Factory.CreateDbContext().Memberships);

            _fixture.Repository.Delete(returnedModel.Id);
            _fixture.TeamRepository.Delete(returnedModel.Team.Id);
            _fixture.MemberRepository.Delete(returnedModel.Member.Id);

            Assert.Empty(_fixture.Factory.CreateDbContext().Teams);
            Assert.Empty(_fixture.Factory.CreateDbContext().Members);
            Assert.Empty(_fixture.Factory.CreateDbContext().Memberships);
        }

        [Fact]
        public void GetById_WithCreatedItem_ReturnedModelMatches()
        {
            var model = new MembershipModel
            {
                IsAdmin = true,
                Id = Guid.NewGuid(),
                IsFounder = true,
                Member = new MemberDetailModel
                {
                    FirstName = "Abraham",
                    LastName = "Lincoln",
                    Nickname = "Strejda",
                    Email = "A.lin5@example.com",
                    Address = "Example Address 3",
                    IsAdmin = false,
                    Id = Guid.NewGuid(),
                    LastActionDate = DateTime.Now
                },
                Team = new TeamDetailModel
                {
                    Name = "Name of some testing Team.",
                    RGB = Color.FromArgb(255, 0, 0),
                    Id = Guid.NewGuid()
                }
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotNull(returnedModel);

            var hopefullyTheSameModel = _fixture.Repository.GetById(returnedModel.Id);

            Assert.True(hopefullyTheSameModel.IsAdmin == returnedModel.IsAdmin && hopefullyTheSameModel.Id == returnedModel.Id);

            _fixture.Repository.Delete(returnedModel.Id);
            _fixture.TeamRepository.Delete(returnedModel.Team.Id);
            _fixture.MemberRepository.Delete(returnedModel.Member.Id);
        }

        [Fact]
        public void GetById_NonExistingItem_NullReturned()
        {
            Assert.Null(_fixture.Repository.GetById(Guid.NewGuid()));
        }

        [Fact]
        public void CreateThenUpdateAndCheck_WithNonExistingItem_UpdatedSuccessfully()
        {
            var model = new MembershipModel
            {
                IsAdmin = true,
                Id = Guid.NewGuid(),
                IsFounder = true,
                Member = new MemberDetailModel
                {
                    FirstName = "Abraham",
                    LastName = "Lincoln",
                    Nickname = "Strejda",
                    Email = "A.lin666@example.com",
                    Address = "Example Address 3",
                    IsAdmin = false,
                    Id = Guid.NewGuid(),
                    LastActionDate = DateTime.Now
                },
                Team = new TeamDetailModel
                {
                    Name = "Name of some testing Team.",
                    RGB = Color.FromArgb(255, 0, 0),
                    Id = Guid.NewGuid()
                }
            };

            var returnedModel = _fixture.Repository.Create(model);
            Assert.NotNull(returnedModel);

            returnedModel.IsAdmin = true;
            _fixture.Repository.Update(returnedModel);

            var updatedModel = _fixture.Repository.GetById(model.Id);

            Assert.True(updatedModel.IsAdmin == returnedModel.IsAdmin && updatedModel.Id == returnedModel.Id);

            _fixture.Repository.Delete(returnedModel.Id);
            _fixture.TeamRepository.Delete(returnedModel.Team.Id);
            _fixture.MemberRepository.Delete(returnedModel.Member.Id);
        }

        [Fact]
        public void UpdateAndDelete()
        {
            var model = new MembershipModel
            {
                IsAdmin = true,
                Id = Guid.NewGuid(),
                IsFounder = true,
                Member = new MemberDetailModel
                {
                    FirstName = "Abraham",
                    LastName = "Lincoln",
                    Nickname = "Strejda",
                    Email = "A.lin666666@example.com",
                    Address = "Example Address 3",
                    IsAdmin = false,
                    Id = Guid.NewGuid(),
                    LastActionDate = DateTime.Now
                },
                Team = new TeamDetailModel
                {
                    Name = "Name of some testing Team.",
                    RGB = Color.FromArgb(255, 0, 0),
                    Id = Guid.NewGuid()
                }
            };

            var returnedModel = _fixture.Repository.Create(model);
            Assert.NotNull(returnedModel);

            returnedModel.IsAdmin = true;
            _fixture.Repository.Update(returnedModel);

            var updatedModel = _fixture.Repository.GetById(model.Id);

            Assert.True(updatedModel.IsAdmin == returnedModel.IsAdmin && updatedModel.Id == returnedModel.Id);

            _fixture.Repository.Delete(returnedModel.Id);
            _fixture.TeamRepository.Delete(returnedModel.Team.Id);
            _fixture.MemberRepository.Delete(returnedModel.Member.Id);

            Assert.Empty(_fixture.Factory.CreateDbContext().Teams);
            Assert.Empty(_fixture.Factory.CreateDbContext().Members);
            Assert.Empty(_fixture.Factory.CreateDbContext().Memberships);
        }
    }
}