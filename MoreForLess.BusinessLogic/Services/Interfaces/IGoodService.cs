using System.Collections.Generic;
using System.Threading.Tasks;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Contains methods for wishlist goods
    /// </summary>
    public interface IGoodService
    {
        /// <summary>
        ///     Add new good to DB.
        /// </summary>
        /// <param name="good">
        ///     New good which is added to DB.
        /// </param>
        /// <returns>
        ///     Good item with id defined by database.
        /// </returns>
        Task<GoodDomainModel> CreateAsync(GoodDomainModel good);

        /// <summary>
        ///     Delete the good in DB.
        /// </summary>
        /// <param name="id">
        ///     Id of good which will be deleted.
        /// </param>
        /// <returns>
        ///     Representing the asynchronous operation.
        /// </returns>
        Task DeleteAsync(int id);

        /// <summary>
        ///     Async method of getting the good by its Id.
        /// </summary>
        /// <param name="id">
        ///     Id of good, which is needed to get.
        /// </param>
        /// <returns>
        ///     Getting good.
        /// </returns>
        Task<GoodDomainModel> GetGoodByIdAsync(int id);

        /// <summary>
        ///     Async method of getting all goods.
        /// </summary>
        /// <returns>
        ///     Getting goods.
        /// </returns>
        Task<IEnumerable<GoodDomainModel>> GetAllGoodsAsync();

        /// <summary>
        ///     Method for checking the good in database by its link.
        /// </summary>
        /// <param name="linkOnProduct">
        ///     Link on good which is needed to check.
        /// </param>
        /// <returns>
        ///     True if the good with such link is exist in database.
        ///     False if the good is not exist.
        /// </returns>
        Task<bool> IsItemExistAsync(string linkOnProduct);
    }
}