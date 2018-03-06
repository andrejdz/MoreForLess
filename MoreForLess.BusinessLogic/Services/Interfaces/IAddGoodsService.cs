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
        /// <exception cref="ArgumentException">
        ///     Throws when getting info from api service.
        /// </exception>
        Task AddGoodsAsync();
    }
}
