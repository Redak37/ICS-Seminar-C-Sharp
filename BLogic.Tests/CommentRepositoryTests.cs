using System;
using BLogic.Models;
using Xunit;

namespace BLogic.Tests
{
    public class CommentRepositoryTests : IClassFixture<CommentRepositoryTestsFixture>
    {
        private readonly CommentRepositoryTestsFixture _fixture;

        public CommentRepositoryTests(CommentRepositoryTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new CommentModel
            {
                Text = "Text of some testing comment.",
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotNull(returnedModel);

            _fixture.Repository.Delete(returnedModel.Id);
        }

        [Fact]
        public void Create_WithNonExistingItemAndEmptySet_DbSetNotEmpty()
        {
            Assert.Empty(_fixture.Factory.CreateDbContext().Comments);

            var model = new CommentModel
            {
                Text = "Text of some testing comment.",
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotEmpty(_fixture.Factory.CreateDbContext().Comments);

            _fixture.Repository.Delete(returnedModel.Id);
        }
        [Fact]
        public void Create_WithNonExistingItem_ModelsMatch()
        {
            var model = new CommentModel
            {
                Text = "Text of some testing comment.",
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.True(model.Id == returnedModel.Id && model.Text == returnedModel.Text);

            _fixture.Repository.Delete(returnedModel.Id);
        }

        [Fact]
        public void DeleteAfterCreating_WithNonExistingItemAndEmptySet_DbSetIsEmpty()
        {
            Assert.Empty(_fixture.Factory.CreateDbContext().Comments);

            var model = new CommentModel
            {
                Text = "Text of some testing comment.",
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotEmpty(_fixture.Factory.CreateDbContext().Comments);

            _fixture.Repository.Delete(returnedModel.Id);

            Assert.Empty(_fixture.Factory.CreateDbContext().Comments);
        }

        [Fact]
        public void GetById_WithCreatedItem_ReturnedModelMatches()
        {
            var model = new CommentModel
            {
                Text = "Text of some testing comment.",
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotNull(returnedModel);

            var hopefullyTheSameModel = _fixture.Repository.GetById(returnedModel.Id);

            Assert.True(returnedModel.Id == hopefullyTheSameModel.Id && returnedModel.Text == hopefullyTheSameModel.Text);

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
            var model = new CommentModel
            {
                Text = "Text of some testing comment.",
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);
            Assert.NotNull(returnedModel);

            returnedModel.Text = "New better updated text.";
            _fixture.Repository.Update(returnedModel);
            
            var updatedModel = _fixture.Repository.GetById(model.Id);

            Assert.True(returnedModel.Id == updatedModel.Id && returnedModel.Text == updatedModel.Text);

            _fixture.Repository.Delete(returnedModel.Id);
        }
    }
}