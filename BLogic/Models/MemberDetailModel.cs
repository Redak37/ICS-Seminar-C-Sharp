using System;

namespace BLogic.Models
{
    public class MemberDetailModel : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
        public DateTime LastActionDate { get; set; }
    }
}