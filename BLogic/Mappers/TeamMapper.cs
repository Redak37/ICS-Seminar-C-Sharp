using BLogic.Models;
using Data;

namespace BLogic.Mappers
{
    public class TeamMapper
    {
        public static TeamDetailModel MapTeamToDetailModel(Team entity)
        {
            return new TeamDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                RGB = entity.RGB
            };
        }

        public static Team MapTeamDetailModelToEntity(TeamDetailModel model)
        {
            return new Team
            {
                Id = model.Id,
                Name = model.Name,
                RGB = model.RGB
            };
        }
    }
}
