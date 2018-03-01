namespace MoreForLess.DataAccess.Entities
{
    public class Good
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string IdGoodOnShop { get; set; }

        public string LinkOnProduct { get; set; }

        public string LinkOnPicture { get; set; }

        public int CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }

        public int ShopId { get; set; }

        public virtual Shop Shop { get; set; }

        public byte[] Timestamp { get; set; }
    }
}