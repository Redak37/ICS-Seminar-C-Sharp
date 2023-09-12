using System.Collections.Generic;

namespace Data
{
    public class Membership : BaseEntity
    {
        public bool IsFounder { get; set; }
        public bool IsAdmin { get; set; }
        public virtual Member Member { get; set; }
        public virtual Team Team { get; set; }

        private sealed class MembershipEqualityComparer : IEqualityComparer<Membership>
        {
            public bool Equals(Membership x, Membership y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                return x?.GetType() == y?.GetType() && x.Id.Equals(y.Id);
            }

            public int GetHashCode(Membership obj)
            {
                var hashCode = obj.Id.GetHashCode();
                return hashCode;
            }
        }

        public static IEqualityComparer<Membership> MembershipComparer { get; } = new MembershipEqualityComparer();
    }
}