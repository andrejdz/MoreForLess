namespace MoreForLess.DataAccess.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public byte[] Timestamp { get; set; }

        public int? GoodId { get; set; }

        public virtual Good Good { get; set; }
    }
}
