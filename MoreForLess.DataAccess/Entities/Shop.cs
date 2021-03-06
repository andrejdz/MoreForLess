﻿using System.Collections.Generic;

namespace MoreForLess.DataAccess.Entities
{
    public class Shop
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Timestamp { get; set; }

        public virtual ICollection<Good> Goods { get; set; } = new HashSet<Good>();

        public virtual ICollection<StoreCategory> StoreCategories { get; set; } = new HashSet<StoreCategory>();
    }
}