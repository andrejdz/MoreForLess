using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentValidation;
using MoreForLess.BusinessLogic.Adapters;
using MoreForLess.BusinessLogic.Adapters.Interfaces;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Services.Interfaces;

namespace MoreForLess.BusinessLogic.Services
{
    /// <inheritdoc />
    public class AddGoodsService : IAddGoodsService
    {
        private readonly IGoodService _goodService;
        private readonly IStoreAdapter<AmazonAdapter> _amazonAdapter;
        private readonly IValidator<RequestParametersModel> _requestParametersModelValidator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddGoodsService"/> class.
        /// </summary>
        /// <param name="goodService">
        ///     Instance of type that implements interface
        ///     <see cref="IGoodService"/>.
        /// </param>
        /// <param name="amazonAdapter">
        ///     Resolver for platform's adapter.
        /// </param>
        /// <param name="requestParametersModelValidator">
        ///     Validator for instance of <see cref="RequestParametersModel"/>.
        /// </param>
        public AddGoodsService(
            IGoodService goodService,
            IStoreAdapter<AmazonAdapter> amazonAdapter,
            IValidator<RequestParametersModel> requestParametersModelValidator)
        {
            this._goodService = goodService;
            this._amazonAdapter = amazonAdapter;
            this._requestParametersModelValidator = requestParametersModelValidator;
        }

        /// <inheritdoc />
        public async Task AddGoodsAsync()
        {
            // Looping with step that equals to 100 (1.00 dollar).
            for (int minPrice = 1000, maxPrice = 1100; maxPrice <= 50_000; minPrice += 100, maxPrice += 100)
            {
                for (int page = 1; page <= 10; page++)
                {
                    var requestParametersModel = new RequestParametersModel
                    {
                        Page = page,
                        MinPrice = minPrice,
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
                    IEnumerable<GoodDomainModel> goodDomainModelsList;
                    try
                    {
                        goodDomainModelsList = await this._amazonAdapter.GetItemInfoByUrlAsync(requestParametersModel);
                    }
                    catch (HttpRequestException ex)
                    {
                        throw new ArgumentException("Error when getting data from api server.", ex);
                    }

                    if (!goodDomainModelsList.Any())
                    {
                        continue;
                    }

                    var goodDomainModelsListDeletedDuplicates = await this.CheckExistenceGoodInDb(goodDomainModelsList);

                    // Adding collection of goods to database.
                    await this._goodService.CreateAsync(goodDomainModelsListDeletedDuplicates);
                }
            }
        }

        private async Task<IEnumerable<GoodDomainModel>> CheckExistenceGoodInDb(IEnumerable<GoodDomainModel> goodDomainModelsList)
        {
            // Checking if good already exist into database.
            // Creating new collection without duplicates.
            var goodDomainModelsListDeletedDuplicates = new List<GoodDomainModel>();
            foreach (var good in goodDomainModelsList)
            {
                if (await this._goodService.IsItemExistAsync(good.IdGoodOnShop) == false)
                {
                    goodDomainModelsListDeletedDuplicates.Add(good);
                }
            }

            return goodDomainModelsListDeletedDuplicates;
        }
    }
}