using System;
using BLogic.Models;
using Xunit;

namespace BLogic.Tests
{
    public class MemberRepositoryTests : IClassFixture<MemberRepositoryTestsFixture>
    {
        private readonly MemberRepositoryTestsFixture _fixture;

        public MemberRepositoryTests(MemberRepositoryTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new MemberDetailModel
            {
                FirstName = "Abraham",
                LastName = "Lincoln",
                Nickname = "",
                Email = "A.lin1@example.com",
                Address = "Example Address 1",
                IsAdmin = false,
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotNull(returnedModel);

            _fixture.Repository.Delete(returnedModel.Id);
        }

        [Fact]
        public void Create_WithNonExistingItemAndEmptySet_DbSetNotEmpty()
        {
            var p = _fixture.Factory.CreateDbContext().Members;
            Assert.Empty(p);

            var model = new MemberDetailModel
            {
                FirstName = "Abraham",
                LastName = "Lincoln",
                Nickname = "",
                Email = "A.lin2@example.com",
                Address = "Example Address 2",
                IsAdmin = false,
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotEmpty(_fixture.Factory.CreateDbContext().Members);

            _fixture.Repository.Delete(returnedModel.Id);
        }
        [Fact]
        public void Create_WithNonExistingItem_ModelsMatch()
        {
            var model = new MemberDetailModel
            {
                FirstName = "Abraham",
                LastName = "Lincoln",
                Nickname = "ZmatenejStrejda",
                Email = "A.lin3@example.com",
                Address = "Example Address 3",
                IsAdmin = false,
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.True(model.IsAdmin == returnedModel.IsAdmin && model.Id == returnedModel.Id && model.Email == returnedModel.Email);
            Assert.True(model.FirstName == returnedModel.FirstName && model.LastName == returnedModel.LastName);
            Assert.True(model.Nickname == returnedModel.Nickname && model.Address == returnedModel.Address);

            _fixture.Repository.Delete(returnedModel.Id);
        }

        [Fact]
        public void DeleteAfterCreating_WithNonExistingItemAndEmptySet_DbSetIsEmpty()
        {
            var p = _fixture.Factory.CreateDbContext().Members;
            Assert.Empty(p);

            var model = new MemberDetailModel
            {
                FirstName = "Abraham",
                LastName = "Lincoln",
                Nickname = "Véči",
                Email = "A.lin4@example.com",
                Address = "Example Address 4",
                IsAdmin = false,
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotEmpty(_fixture.Factory.CreateDbContext().Members);

            _fixture.Repository.Delete(returnedModel.Id);

            Assert.Empty(_fixture.Factory.CreateDbContext().Members);
        }

        [Fact]
        public void GetById_WithCreatedItem_ReturnedModelMatches()
        {
            var model = new MemberDetailModel
            {
                FirstName = "Abraham",
                LastName = "Lincoln",
                Nickname = "Xentax",
                Email = "A.lin5@example.com",
                Address = "Example Address 5",
                IsAdmin = false,
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotNull(returnedModel);

            var hopefullyTheSameModel = _fixture.Repository.GetById(returnedModel.Id);

            Assert.True(hopefullyTheSameModel.IsAdmin == returnedModel.IsAdmin && hopefullyTheSameModel.Id == returnedModel.Id && hopefullyTheSameModel.Email == returnedModel.Email);
            Assert.True(hopefullyTheSameModel.FirstName == returnedModel.FirstName && hopefullyTheSameModel.LastName == returnedModel.LastName);
            Assert.True(hopefullyTheSameModel.Nickname == returnedModel.Nickname && hopefullyTheSameModel.Address == returnedModel.Address);

            _fixture.Repository.Delete(returnedModel.Id);
        }

        [Fact]
        public void GetById_NonExistingItem_NullReturned()
        {
            Assert.Null(_fixture.Repository.GetById(Guid.NewGuid()));
        }

        [Fact]
        public void CreateThenUpdateAndCheck_WithNonExistingItem_UpdatedSuccessfully()
        {
            var model = new MemberDetailModel
            {
                FirstName = "Abraham",
                LastName = "Lincoln",
                Nickname = "Redak",
                Email = "A.lin66@example.com",
                Address = "Example Address 6",
                IsAdmin = false,
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);
            Assert.NotNull(returnedModel);

            returnedModel.FirstName = "New cool FirstName";
            returnedModel.LastName = "New cool LastName";
            returnedModel.Nickname = "New cool Nickname";
            returnedModel.Email = "New cool Email";
            returnedModel.Address = "New cool Address";
            returnedModel.IsAdmin = true;
            _fixture.Repository.Update(returnedModel);

            var updatedModel = _fixture.Repository.GetById(model.Id);

            Assert.True(updatedModel.IsAdmin == returnedModel.IsAdmin && updatedModel.Id == returnedModel.Id && updatedModel.Email == returnedModel.Email);
            Assert.True(updatedModel.FirstName == returnedModel.FirstName && updatedModel.LastName == returnedModel.LastName);
            Assert.True(updatedModel.Nickname == returnedModel.Nickname && updatedModel.Address == returnedModel.Address);

            _fixture.Repository.Delete(returnedModel.Id);
        }

        [Fact]
        public void Password_ModelsMatch()
        {
            var model = new MemberDetailModel
            {
                FirstName = "Abraham",
                LastName = "Lincoln",
                Nickname = "ZmatenejStrejda",
                Email = "A.lin3@example.com",
                Address = "Example Address 3",
                Password = "superTajneHeslo",
                IsAdmin = false,
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);
            
            Assert.True(Password.PasswordCheck("superTajneHeslo", returnedModel?.Password));
            if (returnedModel != null)
            {
                _fixture.Repository.Delete(returnedModel.Id);
            }
        }
    }
}