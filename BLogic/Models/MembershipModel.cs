namespace BLogic.Models
{
    public class MembershipModel : BaseModel
    {
        public bool IsFounder { get; set; }
        public bool IsAdmin { get; set; }
        public virtual MemberDetailModel Member { get; set; }
        public virtual TeamDetailModel Team { get; set; }
    }
}