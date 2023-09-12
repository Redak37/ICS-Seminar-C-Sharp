using System;
using BLogic.Models;
using Xunit;

namespace BLogic.Tests
{
    public class PostRepositoryTests : IClassFixture<PostRepositoryTestsFixture>
    {
        private readonly PostRepositoryTestsFixture _fixture;

        public PostRepositoryTests(PostRepositoryTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new PostDetailModel
            {
                Title = "First post",
                Text = "The most amazing post you have ever seen in your life.",
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotNull(returnedModel);

            _fixture.Repository.Delete(returnedModel.Id);
        }

        [Fact]
        public void Create_WithNonExistingItemAndEmptySet_DbSetNotEmpty()
        {
            Assert.Empty(_fixture.Factory.CreateDbContext().Posts);

            var model = new PostDetailModel
            {
                Title = "Second post",
                Text = "An even more amazing post.",
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotEmpty(_fixture.Factory.CreateDbContext().Posts);

            _fixture.Repository.Delete(returnedModel.Id);
        }

        [Fact]
        public void Create_WithNonExistingItem_ModelsMatch()
        {
            var model = new PostDetailModel
            {
                Title = "Third post",
                Text = "A little more amazing post than the previous one.",
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.True(model.Id == returnedModel.Id);
            Assert.True(model.Text == returnedModel.Text);
            Assert.True(model.Title == returnedModel.Title);

            _fixture.Repository.Delete(returnedModel.Id);
        }

        [Fact]
        public void DeleteAfterCreating_WithNonExistingItemAndEmptySet_DbSetIsEmpty()
        {
            Assert.Empty(_fixture.Factory.CreateDbContext().Posts);

            var model = new PostDetailModel
            {
                Title = "Fourth post",
                Text = "A post that is more amazing than all the previous posts combined.",
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotEmpty(_fixture.Factory.CreateDbContext().Posts);

            _fixture.Repository.Delete(returnedModel.Id);

            Assert.Empty(_fixture.Factory.CreateDbContext().Posts);
        }

        [Fact]
        public void GetById_WithCreatedItem_ReturnedModelMatches()
        {
            var model = new PostDetailModel
            {
                Title = "Fifth post",
                Text = "An almost perfect post.",
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);

            Assert.NotNull(returnedModel);

            var hopefullyTheSameModel = _fixture.Repository.GetById(returnedModel.Id);

            Assert.True(returnedModel.Id == hopefullyTheSameModel.Id);
            Assert.True(returnedModel.Text == hopefullyTheSameModel.Text);
            Assert.True(returnedModel.Title == hopefullyTheSameModel.Title);

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
            var model = new PostDetailModel
            {
                Title = "Sixth post",
                Text = "The perfect post.",
                Id = Guid.NewGuid()
            };

            var returnedModel = _fixture.Repository.Create(model);
            Assert.NotNull(returnedModel);

            returnedModel.Title = "Sixth post (updated)";
            returnedModel.Text = "The perfectest post.";
            _fixture.Repository.Update(returnedModel);

            var updatedModel = _fixture.Repository.GetById(model.Id);

            Assert.True(returnedModel.Id == updatedModel.Id);
            Assert.True(returnedModel.Title == updatedModel.Title);
            Assert.True(returnedModel.Text == updatedModel.Text);

            _fixture.Repository.Delete(returnedModel.Id);
        }
    }
}
