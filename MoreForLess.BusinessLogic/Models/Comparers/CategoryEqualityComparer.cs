using System.Collections.Generic;

namespace MoreForLess.BusinessLogic.Models.Comparers
{
    public class CategoryEqualityComparer : IEqualityComparer<CategoryDomainModel>
    {
        public bool Equals(CategoryDomainModel x, CategoryDomainModel y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null | y == null)
                return false;
            else if (x.IdAtStore.Trim().ToUpperInvariant() == y.IdAtStore.Trim().ToUpperInvariant())
                return true;
            else
                return false;
        }

        public int GetHashCode(CategoryDomainModel obj)
        {
            var hCode = obj.IdAtStore.Length ^ obj.Name.Length ^ obj.ParentIdAtStore.Length;

            return hCode.GetHashCode();
        }
    }
}
