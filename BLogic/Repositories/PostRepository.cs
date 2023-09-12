using System;
using System.Collections.Generic;
using System.Linq;
using BLogic.Factories;
using BLogic.Mappers;
using BLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace BLogic.Repositories
{
    public class PostRepository
    {
        private readonly IDbContextFactory _dbContextFactory;

        public PostRepository(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IEnumerable<PostListModel> GetAllInTeam(TeamDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                return dbContext.Posts.Where(p => p.TeamWithThisPost.Id == model.Id)
                    .Include(c => c.Author)
                    .Include(c => c.TeamWithThisPost)
                    .Select(e => PostMapper.MapPostToListModel(e))
                    .ToList();
            }
        }

        public IEnumerable<PostListModel> GetAllByMember(MemberDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                return dbContext.Posts.Where(p => p.Author.Id == model.Id)
                    .Include(c => c.Author)
                    .Include(c => c.TeamWithThisPost)
                    .Select(e => PostMapper.MapPostToListModel(e))
                    .ToList();
            }
        }

        public PostDetailModel GetById(Guid id)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Posts
                    .Include(c => c.Author)
                    .Include(c => c.TeamWithThisPost)
                    .FirstOrDefault(t => t.Id == id);
                return entity == null ? null : PostMapper.MapPostToDetailModel(entity);
            }
        }

        public PostDetailModel Create(PostDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = PostMapper.MapPostDetailModelToEntity(model);
                var postEntry = dbContext.Posts.Update(entity);
                postEntry.State = EntityState.Added;
                dbContext.SaveChanges();
                return PostMapper.MapPostToDetailModel(entity);
            }
        }

        public void Update(PostDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = PostMapper.MapPostDetailModelToEntity(model);
                dbContext.Posts.Update(entity);
                dbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Posts.First(t => t.Id == id);
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}