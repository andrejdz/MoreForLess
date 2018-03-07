using System;
using System.Threading.Tasks;

namespace MoreForLess.BusinessLogic.Services.Interfaces
{
    /// <summary>
    ///     Types that implement this interface
    ///     add goods info to database.
    /// </summary>
    public interface IAddGoodsService
    {
        /// <summary>
        ///     Adds good's info to database.
        /// </summary>
        /// <param name="minPrice">
        ///     Lower border of price range.
        ///     Used when forming request to store's server.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when getting info from api service.
        /// </exception>
        Task AddGoodsAsync(int minPrice);
    }
}
