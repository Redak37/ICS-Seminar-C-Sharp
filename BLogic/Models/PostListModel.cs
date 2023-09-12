using System;

namespace BLogic.Models
{
    public class PostListModel : BaseModel
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public virtual TeamDetailModel TeamWithThisPost { get; set; }
        public virtual MemberDetailModel Author { get; set; }
        public DateTime LastActivityDate { get; set; }
    }
}
