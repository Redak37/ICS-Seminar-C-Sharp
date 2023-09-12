using BLogic.Models;
using Data;

namespace BLogic.Mappers
{
    public class MembershipMapper
    {
        public static MembershipModel MapMembershipToModel(Membership entity)
        {
            return new MembershipModel
            {
                Id = entity.Id,
                IsFounder = entity.IsFounder,
                IsAdmin = entity.IsAdmin,
                Member = entity.Member == null ? null : MemberMapper.MapMemberToDetailModel(entity.Member),
                Team = entity.Team == null ? null : TeamMapper.MapTeamToDetailModel(entity.Team)
            };
        }

        public static Membership MapMembershipModelToEntity(MembershipModel model)
        {
            return new Membership
            {
                Id = model.Id,
                IsFounder = model.IsFounder,
                IsAdmin = model.IsAdmin,
                Member = model.Member == null ? null : MemberMapper.MapMemberDetailModelToEntity(model.Member),
                Team = model.Team == null ? null : TeamMapper.MapTeamDetailModelToEntity(model.Team)
            };
        }
    }
}