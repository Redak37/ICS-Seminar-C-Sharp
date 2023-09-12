using BLogic.Models;
using Data;

namespace BLogic.Mappers
{
    public class CommentMapper
    {
        public static CommentModel MapCommentToModel(Comment entity)
        {
            return new CommentModel
            {
                Id = entity.Id,
                Text = entity.Text,
                Date = entity.Date,
                Author = entity.Author == null ? null : MemberMapper.MapMemberToDetailModel(entity.Author),
                ParentPost = entity.ParentPost == null ? null : PostMapper.MapPostToDetailModel(entity.ParentPost)
            };
        }

        public static Comment MapCommentModelToEntity(CommentModel model)
        {
            return new Comment
            {
                Id = model.Id,
                Text = model.Text,
                Date = model.Date,
                Author = model.Author == null ? null : MemberMapper.MapMemberDetailModelToEntity(model.Author),
                ParentPost = model.ParentPost == null ? null : PostMapper.MapPostDetailModelToEntity(model.ParentPost)
            };
        }
    }
}
