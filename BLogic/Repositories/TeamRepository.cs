using System;
using System.Collections.Generic;
using System.Linq;
using BLogic.Factories;
using BLogic.Mappers;
using BLogic.Models;

namespace BLogic.Repositories
{
    public class TeamRepository
    {
        private readonly IDbContextFactory _dbContextFactory;

        public TeamRepository(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
      
        public IEnumerable<TeamDetailModel> GetAll()
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                return dbContext.Teams.Select(e => TeamMapper.MapTeamToDetailModel(e))
                    .ToList();
            }
        }

        public IEnumerable<TeamDetailModel> GetAllOfMember(MemberDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                return dbContext.Teams.Where(t => t.Members.Count(ms => ms.Member.Id == model.Id) >= 1)
                    .Select(e => TeamMapper.MapTeamToDetailModel(e))
                    .ToList();
            }
        }

        public TeamDetailModel GetById(Guid id)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Teams.FirstOrDefault(t => t.Id == id);
                return entity == null ? null : TeamMapper.MapTeamToDetailModel(entity);
            }
        }

        public TeamDetailModel Create(TeamDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = TeamMapper.MapTeamDetailModelToEntity(model);
                dbContext.Teams.Add(entity);
                dbContext.SaveChanges();
                return TeamMapper.MapTeamToDetailModel(entity);
            }
        }

        public void Update(TeamDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = TeamMapper.MapTeamDetailModelToEntity(model);
                dbContext.Teams.Update(entity);
                dbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Teams.First(t => t.Id == id);
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }

    }
}
