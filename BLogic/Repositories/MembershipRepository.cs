using System;
using System.Collections.Generic;
using System.Linq;
using BLogic.Factories;
using BLogic.Mappers;
using BLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace BLogic.Repositories
{
    public class MembershipRepository
    {
        private readonly IDbContextFactory _dbContextFactory;

        public MembershipRepository(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IEnumerable<MembershipModel> GetAll()
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                return dbContext.Memberships
                    .Include(c => c.Member)
                    .Include(c => c.Team)
                    .Select(e => MembershipMapper.MapMembershipToModel(e))
                    .ToList();
            }
        }

        public IEnumerable<MembershipModel> GetAllInTeam(TeamDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                return dbContext.Memberships
                    .Include(c => c.Member)
                    .Include(c => c.Team)
                    .Where(m => m.Team.Id == model.Id)
                    .Select(e => MembershipMapper.MapMembershipToModel(e))
                    .ToList();
            }
        }

        public IEnumerable<MembershipModel> GetAllByMember(MemberDetailModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                return dbContext.Memberships
                    .Include(c => c.Member)
                    .Include(c => c.Team)
                    .Where(m => m.Member.Id == model.Id)
                    .Select(e => MembershipMapper.MapMembershipToModel(e))
                    .ToList();
            }
        }

        public MembershipModel GetById(Guid id)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Memberships
                    .Include(c => c.Member)
                    .Include(c => c.Team)
                    .FirstOrDefault(t => t.Id == id);
                return entity == null ? null : MembershipMapper.MapMembershipToModel(entity);
            }
        }

        public MembershipModel GetByTeamAndMemberId(Guid memberId, Guid teamId)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Memberships
                    .Include(c => c.Member)
                    .Include(c => c.Team)
                    .FirstOrDefault(t => t.Team.Id == teamId && t.Member.Id == memberId);
                return entity == null ? null : MembershipMapper.MapMembershipToModel(entity);
            }
        }

        public MembershipModel Create(MembershipModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = MembershipMapper.MapMembershipModelToEntity(model);
                var membershipEntry = dbContext.Memberships.Update(entity);
                membershipEntry.State = EntityState.Added;
                dbContext.SaveChanges();
                return MembershipMapper.MapMembershipToModel(entity);
            }
        }

        public void Update(MembershipModel model)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = MembershipMapper.MapMembershipModelToEntity(model);
                dbContext.Memberships.Update(entity);
                dbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var entity = dbContext.Memberships.First(t => t.Id == id);
                dbContext.Attach(entity);
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}