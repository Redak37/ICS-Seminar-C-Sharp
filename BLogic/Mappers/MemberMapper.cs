using BLogic.Models;
using Data;

namespace BLogic.Mappers
{
    public class MemberMapper
    {
        public static MemberListModel MapMemberToListModel(Member entity)
        {
            return new MemberListModel
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Nickname = entity.Nickname,
                LastActionDate = entity.LastActionDate
            };
        }

        public static MemberDetailModel MapMemberToDetailModel(Member entity)
        {
            return new MemberDetailModel
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Nickname = entity.Nickname,
                Email = entity.Email,
                Address = entity.Address,
                IsAdmin = entity.IsAdmin,
                Password = entity.Password,
                LastActionDate = entity.LastActionDate
            };
        }

        public static Member MapMemberDetailModelToEntity(MemberDetailModel model)
        {
            return new Member
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Nickname = model.Nickname,
                Email = model.Email,
                Address = model.Address,
                IsAdmin = model.IsAdmin,
                Password = model.Password,
                LastActionDate = model.LastActionDate
            };
        }
    }
}