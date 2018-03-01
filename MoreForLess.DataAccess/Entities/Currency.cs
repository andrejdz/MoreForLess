using System.Collections.Generic;

namespace MoreForLess.DataAccess.Entities
{
    public class Currency
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Good> Goods { get; set; } = new HashSet<Good>();

        public byte[] Timestamp { get; set; }
    }
}