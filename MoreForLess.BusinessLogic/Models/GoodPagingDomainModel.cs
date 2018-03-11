using System.Collections.Generic;

namespace MoreForLess.BusinessLogic.Models
{
    /// <summary>
    ///     Contains collection of goods and paging information.
    /// </summary>
    public class GoodPagingDomainModel
    {
        /// <summary>
        ///     Gets or sets collection of goods.
        /// </summary>
        public IEnumerable<GoodDomainModel> Goods { get; set; }

        /// <summary>
        ///     Gets or sets paging information.
        /// </summary>
        public PagingDomainModel PageInfo { get; set; }
    }
}
