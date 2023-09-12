using System;
using System.Collections.Generic;

namespace Data
{
    public class Post : ContentMetadata
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public virtual ICollection<Comment> Comments { get; } = new List<Comment>();
        public virtual Team TeamWithThisPost { get; set; }
        public DateTime LastActivityDate { get; set; }

        private sealed class PostEqualityComparer : IEqualityComparer<Post>
        {
            public bool Equals(Post x, Post y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                return x?.GetType() == y?.GetType() && x.Id.Equals(y.Id);
            }

            public int GetHashCode(Post obj)
            {
                var hashCode = obj.Id.GetHashCode();
                return hashCode;
            }
        }

        public static IEqualityComparer<Post> PostComparer { get; } = new PostEqualityComparer();
    }
}