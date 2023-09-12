namespace BLogic.Messages
{
    public class MemberLogoutMessage : IMessage
    {
        public string Email { get; set; }
    }
}