using System.Collections.Generic;

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

        public string CategoryIdOnShop { get; set; }

        public byte[] Timestamp { get; set; }

        public int CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int ShopId { get; set; }

        public virtual Shop Shop { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<Score> Scores { get; set; } = new HashSet<Score>();
    }
}