using System.Collections.Generic;

namespace MoreForLess.BusinessLogic.Models.Comparers
{
    /// <inheritdoc />
    public class CategoryEqualityComparer : IEqualityComparer<CategoryDomainModel>
    {
        /// <inheritdoc />
        public bool Equals(CategoryDomainModel x, CategoryDomainModel y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null | y == null)
                return false;
            else if (x.IdAtStore.Trim() == y.IdAtStore.Trim())
                return true;
            else
                return false;
        }

        /// <inheritdoc />
        public int GetHashCode(CategoryDomainModel obj)
        {
            return obj.IdAtStore.GetHashCode();
        }
    }
}
