using System.Collections.Generic;
using System.Threading.Tasks;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Services.Interfaces
{
    /// <summary>
    ///     Contains methods for category list.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        ///     Add new categories to database.
        /// </summary>
        /// <param name="categories">
        ///     Collection instances of type <see cref="CategoryDomainModel"/>.
        /// </param>
        /// <param name="shopName">
        ///     Shop's name.
        /// </param>
        Task CreateCategoriesAsync(
            IReadOnlyCollection<CategoryDomainModel> categories,
            string shopName);

        /// <summary>
        ///     Delete category from database.
        /// </summary>
        /// <param name="id">
        ///     Id of category which will be deleted.
        /// </param>
        Task DeleteCategoryAsync(string id);

        /// <summary>
        ///     Async method of getting the category by its id.
        /// </summary>
        /// <param name="id">
        ///     Id of category.
        /// </param>
        /// <returns>
        ///     Instance of type <see cref="CategoryDomainModel"/>.
        /// </returns>
        Task<CategoryDomainModel> GetCategoryByIdAsync(string id);

        /// <summary>
        ///     Async method of getting all categories.
        /// </summary>
        /// <returns>
        ///     Collection of instances of type <see cref="CategoryDomainModel"/>.
        /// </returns>
        Task<IReadOnlyCollection<CategoryDomainModel>> GetAllCategoriesAsync();

        /// <summary>
        ///     Check whether category already added to database by id.
        /// </summary>
        /// <param name="id">
        ///     Category's id at store.
        /// </param>
        /// <returns>
        ///     True if the category with id is exist in database.
        ///     False if the category does not exist.
        /// </returns>
        Task<bool> IsCategoryExistAsync(string id);
    }
}
