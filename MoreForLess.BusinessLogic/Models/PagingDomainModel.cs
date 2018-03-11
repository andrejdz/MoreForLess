using System;

namespace MoreForLess.BusinessLogic.Models
{
    /// <summary>
    ///     Contains info for paging.
    /// </summary>
    public class PagingDomainModel
    {
        /// <summary>
        ///     Gets or sets current page number.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        ///     Gets or sets number of items per page.
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        ///     Gets or sets total number of items.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        ///     Gets total number of pages.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)this.TotalItems / this.ItemsPerPage);
    }
}
