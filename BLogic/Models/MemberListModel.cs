using System;

namespace BLogic.Models
{
    public class MemberListModel : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public DateTime LastActionDate { get; set; }
    }
}