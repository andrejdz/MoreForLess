using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentValidation;
using MoreForLess.BusinessLogic.Adapters;
using MoreForLess.BusinessLogic.Adapters.Interfaces;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Models.Comparers;
using MoreForLess.BusinessLogic.Services.Interfaces;

namespace MoreForLess.BusinessLogic.Services
{
    /// <inheritdoc />
    public class AddGoodsService : IAddGoodsService
    {
        private readonly IGoodService _goodService;
        private readonly ICategoryService _categoryService;
        private readonly IStoreAdapter<AmazonAdapter> _amazonAdapter;
        private readonly IValidator<RequestParametersModel> _requestParametersModelValidator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddGoodsService"/> class.
        /// </summary>
        /// <param name="goodService">
        ///     Instance of type that implements interface
        ///     <see cref="IGoodService"/>.
        /// </param>
        /// <param name="categoryService">
        ///     Instance of type that implements interface
        ///     <see cref="ICategoryService"/>.
        /// </param>
        /// <param name="amazonAdapter">
        ///     Resolver for platform's adapter.
        /// </param>
        /// <param name="requestParametersModelValidator">
        ///     Validator for instance of <see cref="RequestParametersModel"/>.
        /// </param>
        public AddGoodsService(
            IGoodService goodService,
            ICategoryService categoryService,
            IStoreAdapter<AmazonAdapter> amazonAdapter,
            IValidator<RequestParametersModel> requestParametersModelValidator)
        {
            this._goodService = goodService;
            this._categoryService = categoryService;
            this._amazonAdapter = amazonAdapter;
            this._requestParametersModelValidator = requestParametersModelValidator;
        }

        /// <inheritdoc />
        public async Task AddGoodsAsync(int minPrice)
        {
            // Looping with step that equals to 500 (5.00 dollar).
            for (; minPrice <= 50_000; minPrice += 500)
            {
                // Dividing price range into five equal ranges (step 50 or 0.5 dollar).
                for (int page = 1, minPriceDivide = minPrice, maxPrice = minPrice + 50;
                    page <= 10;
                    page++, minPriceDivide += 50, maxPrice += 50)
                {
                    var requestParametersModel = new RequestParametersModel
                    {
                        Page = page,
                        MinPrice = minPriceDivide,
                        MaxPrice = maxPrice
                    };
                    try
                    {
                        // Validating instance of type RequestParametersModel.
                        this._requestParametersModelValidator.ValidateAndThrow(requestParametersModel);
                    }
                    catch (ValidationException ex)
                    {
                        throw new ArgumentException($"Error when validating {requestParametersModel}.", ex);
                    }

                    // Getting good's info from server and adding to collection.
                    IReadOnlyCollection<GoodDomainModel> goodDomainModels;
                    try
                    {
                        goodDomainModels = await this._amazonAdapter.GetItemInfoByUrlAsync(requestParametersModel);
                    }
                    catch (HttpRequestException ex)
                    {
                        throw new ArgumentException("Error when getting data from api server.", ex);
                    }

                    if (!goodDomainModels.Any())
                    {
                        continue;
                    }

                    // Checking if good already exist into database.
                    // Creating new collection without duplicates.
                    var goodDomainModelsDeletedDuplicates = await this.CheckExistenceGoodInDb(goodDomainModels);

                    // Checking if category already exist into database.
                    // Creating new collection without duplicates.
                    IReadOnlyCollection<CategoryDomainModel> categoryDomainModelsDeletedDuplicates = 
                        await this.CheckExistenceCategoryInDb(goodDomainModels.SelectMany(g => g.CategoryIdsOnShop));

                    // Adding collection of goods to database.
                    if (goodDomainModelsDeletedDuplicates.Any())
                    {
                        // Adding collection of categories to database.
                        if (categoryDomainModelsDeletedDuplicates.Any())
                        {
                            await this._categoryService.CreateCategoriesAsync(
                                categoryDomainModelsDeletedDuplicates,
                                goodDomainModelsDeletedDuplicates.First().ShopName);
                        }

                        await this._goodService.CreateAsync(goodDomainModelsDeletedDuplicates);
                    }
                }
            }
        }

        private async Task<IReadOnlyCollection<GoodDomainModel>> CheckExistenceGoodInDb(
            IReadOnlyCollection<GoodDomainModel> goodDomainModels)
        {
            var goodDomainModelsDeletedDuplicates = new List<GoodDomainModel>();
            foreach (var good in goodDomainModels)
            {
                if (await this._goodService.IsItemExistAsync(good.IdGoodOnShop) == false)
                {
                    goodDomainModelsDeletedDuplicates.Add(good);
                }
            }

            return goodDomainModelsDeletedDuplicates;
        }

        private async Task<IReadOnlyCollection<CategoryDomainModel>> CheckExistenceCategoryInDb(
            IEnumerable<CategoryDomainModel> categoryDomainModels)
        {
            var categoryDomainModelsDistinct = categoryDomainModels.Distinct(new CategoryEqualityComparer()).ToList();

            var categoryDomainModelsDeletedDuplicates = new List<CategoryDomainModel>();
            foreach (var category in categoryDomainModelsDistinct)
            {
                if (await this._categoryService.IsCategoryExistAsync(category.IdAtStore) == false)
                {
                    categoryDomainModelsDeletedDuplicates.Add(category);
                }
            }

            return categoryDomainModelsDeletedDuplicates;
        }
    }
}