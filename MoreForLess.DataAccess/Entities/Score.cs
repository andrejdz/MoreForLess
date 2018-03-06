namespace MoreForLess.DataAccess.Entities
{
    public class Score
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public byte[] Timestamp { get; set; }

        public int? GoodId { get; set; }

        public virtual Good Good { get; set; }
    }
}
