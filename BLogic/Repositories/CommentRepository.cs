using System;
using System.Collections.Generic;
using System.Linq;
using BLogic.Factories;
using BLogic.Mappers;
using BLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace BLogic.Repositories
{
    public class CommentRepository
    {
        private readonly IDbContextFactory _dbContextFactory;

        public CommentRepository(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IEnumerable<CommentModel> GetAllInPost(PostDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                return dbContext.Comments.Where(c => c.ParentPost.Id == model.Id)
                    .Include(c => c.Author)
                    .Include(c => c.ParentPost)
                    .Select(e => CommentMapper.MapCommentToModel(e))
                    .ToList();
            }
        }

        public IEnumerable<CommentModel> GetAllByMember(MemberDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                return dbContext.Comments.Where(c => c.Author.Id == model.Id)
                    .Include(c => c.Author)
                    .Include(c => c.ParentPost)
                    .Select(e => CommentMapper.MapCommentToModel(e))
                    .ToList();
            }
        }

        public CommentModel GetById(Guid id)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Comments
                    .Include(c => c.Author)
                    .Include(c => c.ParentPost)
                    .FirstOrDefault(t => t.Id == id);
                return entity == null ? null : CommentMapper.MapCommentToModel(entity);
            }
        }

        public CommentModel Create(CommentModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = CommentMapper.MapCommentModelToEntity(model);
                if (entity.Author.Id == entity.ParentPost.Author.Id)
                {
                    entity.ParentPost.Author = null;
                }
                var commentEntry = dbContext.Comments.Attach(entity);
                commentEntry.State = EntityState.Added;
                dbContext.SaveChanges();
                return CommentMapper.MapCommentToModel(entity);
            }
        }

        public void Update(CommentModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = CommentMapper.MapCommentModelToEntity(model);
                dbContext.Comments.Update(entity);
                dbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Comments.First(t => t.Id == id);
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
