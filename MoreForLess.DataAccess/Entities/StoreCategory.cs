using System.Collections.Generic;

namespace MoreForLess.DataAccess.Entities
{
    public class StoreCategory
    {
        public string IdAtStore { get; set; }

        public string Name { get; set; }

        public string ParentIdAtStore { get; set; }

        public virtual StoreCategory Parent { get; set; }

        public virtual ICollection<StoreCategory> Children { get; set; } = new HashSet<StoreCategory>();

        public byte[] Timestamp { get; set; }

        public int ShopId { get; set; }

        public virtual Shop Shop { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
