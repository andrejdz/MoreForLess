using System.Collections.Generic;

namespace MoreForLess.BusinessLogic.Models
{
    /// <summary>
    ///     Type that stores category's info.
    /// </summary>
    public class CategoryDomainModel
    {
        /// <summary>
        ///     Category's id at store.
        /// </summary>
        public string IdAtStore { get; set; }

        /// <summary>
        ///     Good's category name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Id of parent category.
        /// </summary>
        public string ParentIdAtStore { get; set; }

        /// <summary>
        ///     Gets or sets children categories.
        /// </summary>
        public IReadOnlyCollection<CategoryDomainModel> ChildrenCategories { get; set; }
    }
}
