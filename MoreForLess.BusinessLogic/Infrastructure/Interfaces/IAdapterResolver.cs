using System;
using MoreForLess.BusinessLogic.Adapters.Interfaces;

namespace MoreForLess.BusinessLogic.Infrastructure.Interfaces
{
    public interface IAdapterResolver
    {
        /// <summary>
        ///     Resolves what adapter to use.
        /// </summary>
        /// <param name="platform">
        ///     Name of platform.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when platform isn't supported
        /// </exception>
        /// <returns>
        ///     Necessary instance of adapter.
        /// </returns>
        IStoreAdapter Resolve(string platform);
    }
}
