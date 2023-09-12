using System;
using BLogic.Models;
using Xunit;
using System.Drawing;

namespace BLogic.Tests
{
    public class TeamRepositoryTests : IClassFixture<TeamRepositoryTestsFixture>
    {
        private readonly TeamRepositoryTestsFixture _fixture;

        public TeamRepositoryTests(TeamRepositoryTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new TeamDetailModel
            {
                Name = "Name of some testing Team.",
                RGB = Color.FromArgb(255, 0, 0),
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotNull(returnedModel);

            _fixture.Repository.Delete(returnedModel.Id);
        }

        [Fact]
        public void Create_WithNonExistingItemAndEmptySet_DbSetNotEmpty()
        {
            Assert.Empty(_fixture.Factory.CreateDbContext().Teams);

            var model = new TeamDetailModel
            {
                Name = "Name of some testing Team.",
                RGB = Color.FromArgb(255, 0, 0),
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotEmpty(_fixture.Factory.CreateDbContext().Teams);

            _fixture.Repository.Delete(returnedModel.Id);
        }
        [Fact]
        public void Create_WithNonExistingItem_ModelsMatch()
        {
            var model = new TeamDetailModel
            {
                Name = "Name of some testing Team.",
                RGB = Color.FromArgb(255, 0, 0),
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.True(model.Id == returnedModel.Id);
            Assert.True(model.Name == returnedModel.Name);
            Assert.True(model.RGB == Color.FromArgb(255, 0, 0));

            _fixture.Repository.Delete(returnedModel.Id);
        }

        [Fact]
        public void DeleteAfterCreating_WithNonExistingItemAndEmptySet_DbSetIsEmpty()
        {
            var p = _fixture.Factory.CreateDbContext().Teams;
            Assert.Empty(p);

            var model = new TeamDetailModel
            {
                Name = "Name of some testing Team.",
                RGB = Color.FromArgb(255, 0, 0),
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotEmpty(_fixture.Factory.CreateDbContext().Teams);

            _fixture.Repository.Delete(returnedModel.Id);

            Assert.Empty(_fixture.Factory.CreateDbContext().Teams);
        }

        [Fact]
        public void GetById_WithCreatedItem_ReturnedModelMatches()
        {
            var model = new TeamDetailModel
            {
                Name = "Name of some testing Team.",
                RGB = Color.FromArgb(255, 0, 0),
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotNull(returnedModel);

            var hopefullyTheSameModel = _fixture.Repository.GetById(returnedModel.Id);

            Assert.True(returnedModel.Id == hopefullyTheSameModel.Id);
            Assert.True(returnedModel.Name == hopefullyTheSameModel.Name);
            Assert.True(returnedModel.RGB == hopefullyTheSameModel.RGB);

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
            var model = new TeamDetailModel
            {
                Name = "Name of some testing Team.",
                RGB = Color.FromArgb(255, 0, 0),
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);
            Assert.NotNull(returnedModel);

            returnedModel.Name = "New better updated Name.";
            returnedModel.RGB = Color.FromArgb(0, 0, 255);
            _fixture.Repository.Update(returnedModel);

            var updatedModel = _fixture.Repository.GetById(model.Id);

            Assert.True(returnedModel.Id == updatedModel.Id);
            Assert.True(returnedModel.Name == updatedModel.Name);
            Assert.True(returnedModel.RGB == updatedModel.RGB);

            _fixture.Repository.Delete(returnedModel.Id);
        }
    }
}