using BLogic.Models;

namespace BLogic.Messages
{
    public class MemberLoginMessage : IMessage
    {
        public MemberDetailModel LoggedInMember { get; set; }
    }
}