using System;
using System.Collections.Generic;
using System.Linq;
using BLogic.Factories;
using BLogic.Mappers;
using BLogic.Models;

namespace BLogic.Repositories
{
    public class MemberRepository
    {
        private readonly IDbContextFactory _dbContextFactory;

        public MemberRepository(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        
        public IEnumerable<MemberListModel> GetAll()
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                return dbContext.Members
                    .Select(e => MemberMapper.MapMemberToListModel(e))
                    .ToList();
            }
        }

        public IEnumerable<MemberListModel> GetAllInTeam(TeamDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                return dbContext.Members.Where(m => 
                        dbContext.Memberships.Any(ms => ms.Team.Id == model.Id && ms.Member.Id == m.Id))
                    .Select(e => MemberMapper.MapMemberToListModel(e))
                    .ToList();
            }
        }

        public MemberDetailModel GetById(Guid id)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Members.FirstOrDefault(t => t.Id == id);
                return entity == null ? null : MemberMapper.MapMemberToDetailModel(entity);
            }
        }

        public MemberDetailModel GetByEmail(string email)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Members.FirstOrDefault(t => t.Email == email);
                return entity == null ? null : MemberMapper.MapMemberToDetailModel(entity);
            }
        }

        public MemberListModel GetByIdListModel(Guid id)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Members.FirstOrDefault(t => t.Id == id);
                return entity == null ? null : MemberMapper.MapMemberToListModel(entity);
            }
        }

        public MemberDetailModel Create(MemberDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = MemberMapper.MapMemberDetailModelToEntity(model);
                dbContext.Members.Add(entity);
                dbContext.SaveChanges();
                return MemberMapper.MapMemberToDetailModel(entity);
            }
        }

        public void Update(MemberDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = MemberMapper.MapMemberDetailModelToEntity(model);
                dbContext.Members.Update(entity);
                dbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Members.First(t => t.Id == id);
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}