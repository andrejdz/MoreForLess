using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using NLog;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Services.Interfaces;
using MoreForLess.DataAccess.EF;
using MoreForLess.DataAccess.Entities;

namespace MoreForLess.BusinessLogic.Services
{
    /// <summary>
    /// Repository for wishlist goods.
    /// </summary>
    public class GoodService : IGoodService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<GoodDomainModel> _goodDomainModelValidator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GoodService"/> class.
        /// </summary>
        /// <param name="context">
        ///     Field thanks to which we can interact with the database.
        /// </param>
        /// <param name="mapper">
        ///     Instance of <see cref="Mapper"/>.
        /// </param>
        /// <param name="goodDomainModelValidator">
        ///     Instance of type that implements interface
        ///     <see cref="IValidator{GoodDomainModel}"/>.
        /// </param>
        public GoodService(
            ApplicationDbContext context,
            IMapper mapper,
            IValidator<GoodDomainModel> goodDomainModelValidator)
        {
            this._context = context;
            this._mapper = mapper;
            this._goodDomainModelValidator = goodDomainModelValidator;
        }

        /// <summary>
        ///     Adds new good to database.
        /// </summary>
        /// <param name="goodDomainModel">
        ///     New good which is added to database.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when shop or currency don't exist in database.
        ///     Throws when validation is failed.
        /// </exception>
        /// <returns>
        ///     Good item with id defined by database.
        /// </returns>
        public async Task<GoodDomainModel> CreateAsync(GoodDomainModel goodDomainModel)
        {
            try
            {
                this._goodDomainModelValidator.ValidateAndThrow(goodDomainModel);
            }
            catch (ValidationException ex)
            {
                throw new ArgumentException($"Error when validating {goodDomainModel}.", ex);
            }

            var good = this._mapper.Map<Good>(goodDomainModel);

            _logger.Info($"Retrieving currency id of {goodDomainModel.CurrencyName}.");
            int currencyId;
            try
            {
                var currency = await this._context.Currencies
                    .SingleAsync(u => u.Name == goodDomainModel.CurrencyName);

                currencyId = currency.Id;
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Error when getting currency's id", ex);
            }

            _logger.Info($"Retrieving shop id of {goodDomainModel.ShopName}.");
            int shopId;
            try
            {
                var shop = await this._context.Shops
                    .SingleAsync(u => u.Name == goodDomainModel.ShopName);

                shopId = shop.Id;
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Error when getting shop's id.", ex);
            }

            good.CurrencyId = currencyId;
            good.ShopId = shopId;

            this._context.Goods.Add(good);

            _logger.Info($"Saving good item: {good.Name} into database.");

            await this._context.SaveChangesAsync();

            return this._mapper.Map<GoodDomainModel>(good);
        }

        /// <summary>
        ///     Delete the good from database.
        /// </summary>
        /// <param name="id">
        ///     Id of good which will be deleted.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Good doesn't exist in database.
        /// </exception>
        /// <returns>
        ///     Representing the asynchronous operation.
        /// </returns>
        public async Task DeleteAsync(int id)
        {
            _logger.Info($"Looking for good item by id: {id}.");
            Good good;
            try
            {
                good = await this._context.Goods.SingleAsync(u => u.Id == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Good hasn't been found.", ex);
            }

            _logger.Info($"Removing good item: {good.Name} from database.");
            this._context.Goods.Remove(good);

            await this._context.SaveChangesAsync();
        }

        /// <summary>
        ///     Async method for getting the good by its id.
        /// </summary>
        /// <param name="id">
        ///     Id of good, which is needed to get.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when good doesn't exist in database.
        /// </exception>
        /// <returns>
        ///     Getting good.
        /// </returns>
        public async Task<GoodDomainModel> GetGoodByIdAsync(int id)
        {
            _logger.Info($"Looking for good item by id: {id}.");
            Good good;
            try
            {
                good = await this._context.Goods.Where(item => item.Id == id).SingleAsync();
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Good hasn't been found.", ex);
            }

            return this._mapper.Map<GoodDomainModel>(good);
        }

        /// <summary>
        ///     Async method to get all goods from database.
        /// </summary>
        /// <returns>
        ///     Getting goods.
        /// </returns>
        public async Task<IEnumerable<GoodDomainModel>> GetAllGoodsAsync()
        {
            _logger.Info("Retrieving collection of good items.");
            List<Good> goods = await this._context.Goods.ToListAsync();

            var returnedGoods = goods.Select(good => this._mapper.Map<GoodDomainModel>(good));

            return returnedGoods;
        }

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
        public async Task<bool> IsItemExistAsync(string linkOnProduct)
        {
            _logger.Info($"Checking whether good item with url: {linkOnProduct} has already been added to database.");

            try
            {
                await this._context.Goods.SingleAsync(item => item.LinkOnProduct == linkOnProduct);

                _logger.Info($"Good with url: {linkOnProduct} exists in database.");
                return true;
            }
            catch (InvalidOperationException)
            {
                _logger.Info($"Good with url: {linkOnProduct} doesn't exist in database.");
                return false;
            }
        }
    }
}