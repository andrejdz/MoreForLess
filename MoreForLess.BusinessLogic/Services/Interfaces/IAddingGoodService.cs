using System;
using System.Threading.Tasks;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Services.Interfaces
{
    /// <summary>
    ///     Interface contains contract.
    ///     Types that implements this interface have to realize its.
    /// </summary>
    public interface IAddingGoodService
    {
        /// <summary>
        ///     Adds good item to database.
        /// </summary>
        /// <param name="url">
        ///     Url of good on store's site.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when good's url is null, empty or
        ///     contains only white-space characters.
        /// </exception>
        /// <returns>
        ///     Good item with id defined by database.
        /// </returns>
        Task<GoodDomainModel> ProcessAsync(string url);
    }
}