using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreForLess.BusinessLogic.Helpers
{
    public static class FlattenExtension
    {
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> e, Func<T, IEnumerable<T>> f)
        {
            return e.SelectMany(c => f(c).Flatten(f)).Concat(e);
        }
    }
}
