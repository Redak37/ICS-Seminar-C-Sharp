using System;

namespace BLogic.Models
{
    public class CommentModel : BaseModel
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public virtual PostDetailModel ParentPost { get; set; }
        public virtual MemberDetailModel Author { get; set; }
    }
}
