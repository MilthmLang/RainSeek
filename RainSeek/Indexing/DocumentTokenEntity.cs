using System;

namespace RainSeek.Indexing
{
    public class DocumentTokenEntity
    {
        public long Id { get; set; }

        public long TokenId { get; set; }

        public string DocumentId { get; set; }

        public int StartPosition { get; set; }

        public int EndPosition { get; set; }
    }
}
