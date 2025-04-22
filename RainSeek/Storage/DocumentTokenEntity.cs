namespace RainSeek.Storage
{
    public class DocumentTokenEntity
    {
        public long ID { get; set; }

        public long TokenID { get; set; }

        public string DocumentID { get; set; }

        public int StartPosition { get; set; }

        public int EndPosition { get; set; }
    }
}
