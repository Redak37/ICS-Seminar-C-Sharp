using BLogic.Models;

namespace BLogic.Messages
{
    public class ShowMemberInfoMessage : IMessage
    {
        public MemberDetailModel ViewedMember { get; set; }
        public MemberDetailModel ActiveMember { get; set; }
    }
}