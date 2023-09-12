using BLogic.Models;

namespace BLogic.Messages
{
    public class ActiveTeamChangedMessage : IMessage
    {
        public TeamDetailModel ActiveTeam;
    }
}