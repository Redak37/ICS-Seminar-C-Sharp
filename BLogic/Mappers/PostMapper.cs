using BLogic.Models;
using Data;

namespace BLogic.Mappers
{
    public class PostMapper
    {
        public static PostListModel MapPostToListModel(Post entity)
        {
            return new PostListModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Date = entity.Date,
                Author = entity.Author == null ? null : MemberMapper.MapMemberToDetailModel(entity.Author),
                TeamWithThisPost = entity.TeamWithThisPost == null ? null : TeamMapper.MapTeamToDetailModel(entity.TeamWithThisPost),
                LastActivityDate = entity.LastActivityDate
            };
        }

        public static PostDetailModel MapPostToDetailModel(Post entity)
        {
            return new PostDetailModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Text = entity.Text,
                Date = entity.Date,
                Author = entity.Author == null ? null : MemberMapper.MapMemberToDetailModel(entity.Author),
                TeamWithThisPost = entity.TeamWithThisPost == null ? null : TeamMapper.MapTeamToDetailModel(entity.TeamWithThisPost),
                LastActivityDate = entity.LastActivityDate
            };
        }

        public static Post MapPostDetailModelToEntity(PostDetailModel model)
        {
            return new Post
            {
                Id = model.Id,
                Title = model.Title,
                Text = model.Text,
                Date = model.Date,
                Author = model.Author == null ? null : MemberMapper.MapMemberDetailModelToEntity(model.Author),
                TeamWithThisPost = model.TeamWithThisPost == null ? null : TeamMapper.MapTeamDetailModelToEntity(model.TeamWithThisPost),
                LastActivityDate = model.LastActivityDate
            };
        }
    }
}
