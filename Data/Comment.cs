using System.Collections.Generic;

namespace Data
{
    public class Comment : ContentMetadata
    {
        public string Text { get; set; }
        public virtual Post ParentPost { get; set; }

        private sealed class CommentEqualityComparer : IEqualityComparer<Comment>
        {
            public bool Equals(Comment x, Comment y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                return x?.GetType() == y?.GetType() && x.Id.Equals(y.Id);
            }

            public int GetHashCode(Comment obj)
            {
                var hashCode = obj.Id.GetHashCode();
                return hashCode;
            }
        }

        public static IEqualityComparer<Comment> CommentComparer { get; } = new CommentEqualityComparer();
    }
}