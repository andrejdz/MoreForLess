namespace MoreForLess.DataAccess.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public byte[] Timestamp { get; set; }
    }
}
