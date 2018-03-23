using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Services.Interfaces;
using MoreForLess.DataAccess.EF;
using MoreForLess.DataAccess.Entities;
using NLog;

namespace MoreForLess.BusinessLogic.Services
{
    /// <inheritdoc />
    public class CategoryService : ICategoryService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryDomainModel> _categoryDomainModelValidator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="context">
        ///     Context of database.
        /// </param>
        /// <param name="mapper">
        ///     Instance of <see cref="Mapper"/>.
        /// </param>
        /// <param name="categoryDomainModelValidator">
        ///     Instance of type that implements interface
        ///     <see cref="IValidator{CategoryDomainModel}"/>.
        /// </param>
        public CategoryService(
            ApplicationDbContext context,
            IMapper mapper,
            IValidator<CategoryDomainModel> categoryDomainModelValidator)
        {
            this._context = context;
            this._mapper = mapper;
            this._categoryDomainModelValidator = categoryDomainModelValidator;
        }

        /// <inheritdoc />
        public async Task CreateCategoriesAsync(
            IReadOnlyCollection<CategoryDomainModel> categoryDomainModels,
            string shopName)
        {
            _logger.Info($"Retrieving shop id of {shopName}.");
            int shopId;
            try
            {
                var shop = await this._context.Shops
                    .SingleAsync(u => u.Name == shopName);

                shopId = shop.Id;
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Error when getting shop's id.", ex);
            }

            List<StoreCategory> storeCategories = new List<StoreCategory>();
            foreach (var categoryDomainModel in categoryDomainModels)
            {
                try
                {
                    this._categoryDomainModelValidator.ValidateAndThrow(categoryDomainModel);
                }
                catch (ValidationException ex)
                {
                    throw new ArgumentException($"Error when validating {categoryDomainModel}.", ex);
                }

                var storeCategory = this._mapper.Map<StoreCategory>(categoryDomainModel);

                storeCategory.ShopId = shopId;

                _logger.Info($"Adding store's category: {storeCategory.Name} into collection.");
                storeCategories.Add(storeCategory);
            }

            _logger.Info($"Saving store's categories into database.");
            this._context.StoreCategories.AddRange(storeCategories);

            await this._context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteCategoryAsync(string id)
        {
            _logger.Info($"Looking for category by id: {id}.");
            StoreCategory storeCategory;
            try
            {
                storeCategory = await this._context.StoreCategories.SingleAsync(c => c.IdAtStore == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Good hasn't been found.", ex);
            }

            _logger.Info($"Removing category: {storeCategory.Name} from database.");
            this._context.StoreCategories.Remove(storeCategory);

            await this._context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<CategoryDomainModel> GetCategoryByIdAsync(string id)
        {
            _logger.Info($"Getting category by its id: {id}.");
            StoreCategory storeCategory;
            try
            {
                storeCategory = await this._context.StoreCategories
                    .Where(c => c.IdAtStore == id)
                    .SingleAsync();
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Category hasn't been found.", ex);
            }

            return this._mapper.Map<CategoryDomainModel>(storeCategory);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<CategoryDomainModel>> GetAllCategoriesAsync()
        {
            _logger.Info($"Getting all categories.");
            IReadOnlyCollection<StoreCategory> storeCategories = await this._context.StoreCategories
                .Where(c => c.ParentIdAtStore == null)
                .ToListAsync();

            return this._mapper.Map<IReadOnlyCollection<CategoryDomainModel>> (storeCategories);
        }

        /// <inheritdoc />
        public async Task<bool> IsCategoryExistAsync(string id)
        {
            _logger.Info($"Checking whether category with id: {id} has already been added to database.");

            try
            {
                await this._context.StoreCategories.SingleAsync(c => c.IdAtStore == id);

                _logger.Info($"Category with id: {id} exists in the database.");
                return true;
            }
            catch (InvalidOperationException)
            {
                _logger.Info($"Category with id: {id} doesn't exist in the database.");
                return false;
            }
        }

        /// <inheritdoc />
        public async Task<StoreCategory> GetCategoryByIdAltAsync(string id)
        {
            _logger.Info($"Getting category by its id: {id}.");
            StoreCategory storeCategory;
            try
            {
                storeCategory = await this._context.StoreCategories
                    .Where(c => c.IdAtStore == id)
                    .SingleAsync();
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Category hasn't been found.", ex);
            }

            return storeCategory;
        }

        /// <inheritdoc />
        public async Task<bool> IsCategoryInInheritanceChain(string categoryId, string goodId)
        {
            _logger.Info($"Getting category by its id: {goodId}.");

            StoreCategory storeCategory = await this._context.StoreCategories
                .SingleAsync(c => c.IdAtStore == goodId);

            if (storeCategory.IdAtStore == categoryId)
            {
                return true;
            }
            else
            {
                return this.LookingForCategory(categoryId, storeCategory.Parent);
            }
        }

        private bool LookingForCategory(string categoryId, StoreCategory storeCategory)
        {
            if (storeCategory.IdAtStore == categoryId)
            {
                return true;
            }
            else if (storeCategory.ParentIdAtStore == null)
            {
                return false;
            }
            else
            {
                return this.LookingForCategory(categoryId, storeCategory.Parent);
            }
        }
    }
}
