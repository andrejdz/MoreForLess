namespace MoreForLess.BusinessLogic.Models
{
    /// <summary>
    ///     Contains request's parameters.
    /// </summary>
    public class PageCategoryModel
    {
        /// <summary>
        ///     Number of current page.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        ///     Items per page.
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        ///     Category id at store.
        /// </summary>
        public string CategoryId { get; set; }
    }
}
