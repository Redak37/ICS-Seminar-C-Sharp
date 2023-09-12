using System;

namespace BLogic.Models
{
    public class PostDetailModel : BaseModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public virtual TeamDetailModel TeamWithThisPost { get; set; }
        public virtual MemberDetailModel Author { get; set; }
        public DateTime LastActivityDate { get; set; }
    }
}
