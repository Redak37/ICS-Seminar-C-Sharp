using System;
using System.Collections.Generic;

namespace Data
{
    public class Member : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
        public DateTime LastActionDate { get; set; }
        public virtual ICollection<Post> Posts { get; } = new List<Post>();
        public virtual ICollection<Comment> Comments { get; } = new List<Comment>();
        public virtual ICollection<Membership> Teams { get; set; } = new List<Membership>();

        private sealed class MemberEqualityComparer : IEqualityComparer<Member>
        {
            public bool Equals(Member x, Member y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                return x?.GetType() == y?.GetType() && x.Id.Equals(y.Id);
            }

            public int GetHashCode(Member obj)
            {
                var hashCode = obj.Id.GetHashCode();
                return hashCode;
            }
        }

        public static IEqualityComparer<Member> MemberComparer { get; } = new MemberEqualityComparer();
    }
}