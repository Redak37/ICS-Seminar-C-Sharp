using System;

namespace Data
{
    public class ContentMetadata : BaseEntity
    {
        public DateTime Date { get; set; }
        public virtual Member Author { get; set; }
    }
}