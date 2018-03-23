using System.Collections.Generic;
using System.Threading.Tasks;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Contains methods for wish-list goods.
    /// </summary>
    public interface IGoodService
    {
        /// <summary>
        ///     Add new good to DB.
        /// </summary>
        /// <returns>
        ///     Good item with id defined by database.
        /// </returns>
        Task CreateAsync(IReadOnlyCollection<GoodDomainModel> goodList);

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
        /// <param name="pageCategoryModel">
        ///     Contains request's parameters.
        /// </param>
        /// <returns>
        ///     Collection of goods and paging information.
        /// </returns>
        Task<GoodPagingDomainModel> GetAllGoodsAsync(PageCategoryModel pageCategoryModel);

        /// <summary>
        ///     Method for checking the good in database by its link.
        /// </summary>
        /// <param name="idGoodOnShop">
        ///     Good's id at store which is needed to check.
        /// </param>
        /// <returns>
        ///     True if the good with id is exist in database.
        ///     False if the good is not exist.
        /// </returns>
        Task<bool> IsItemExistAsync(string idGoodOnShop);
    }
}