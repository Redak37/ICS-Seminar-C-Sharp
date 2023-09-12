using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Data
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }
        public int Argb { get; set; }

        [NotMapped]
        public Color RGB
        {
            get => Color.FromArgb(Argb);
            set => Argb = value.ToArgb();
        }
        public virtual ICollection<Membership> Members { get; } = new List<Membership>();
        public virtual ICollection<Post> Posts { get; } = new List<Post>();
        
        private sealed class TeamEqualityComparer : IEqualityComparer<Team>
        {
            public bool Equals(Team x, Team y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                return x?.GetType() == y?.GetType() && x.Id.Equals(y.Id);
            }

            public int GetHashCode(Team obj)
            {
                var hashCode = obj.Id.GetHashCode();
                return hashCode;
            }
        }

        public static IEqualityComparer<Team> TeamComparer { get; } = new TeamEqualityComparer();
    }
}