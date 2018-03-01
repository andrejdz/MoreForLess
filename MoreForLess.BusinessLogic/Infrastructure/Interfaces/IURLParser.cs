using System;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Infrastructure.Interfaces
{
    public interface IURLParser
    {
        /// <summary>
        ///     Gets a platform and an item id from the url.
        /// </summary>
        /// <param name="url">
        ///     An url of the item.
        /// </param>
        /// <returns>
        ///     A URLInfo object that contains platform and item id.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Throws when url is null or empty.
        ///     Throws when provided platform isn't supported.
        ///     Throws when item id can't be found in URL.
        /// </exception>
        /// <exception cref="FormatException">
        ///     Throws when url has invalid format.
        /// </exception>
        URLInfo Parse(string url);
    }
}
