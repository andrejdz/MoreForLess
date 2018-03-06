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
    /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task CreateAsync(IEnumerable<GoodDomainModel> goodDomainModelCollection)
        {
            foreach (var goodDomainModel in goodDomainModelCollection)
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
            }

            await this._context.SaveChangesAsync();
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task<IEnumerable<GoodDomainModel>> GetAllGoodsAsync()
        {
            _logger.Info("Retrieving collection of good items.");
            List<Good> goods = await this._context.Goods.ToListAsync();

            var returnedGoods = goods.Select(good => this._mapper.Map<GoodDomainModel>(good));

            return returnedGoods;
        }

        /// <inheritdoc />
        public async Task<bool> IsItemExistAsync(string idGoodOnShop)
        {
            _logger.Info($"Checking whether good item with id: {idGoodOnShop} has already been added to database.");

            try
            {
                await this._context.Goods.SingleAsync(item => item.IdGoodOnShop == idGoodOnShop);

                _logger.Info($"Good with id: {idGoodOnShop} exists in database.");
                return true;
            }
            catch (InvalidOperationException)
            {
                _logger.Info($"Good with id: {idGoodOnShop} doesn't exist in database.");
                return false;
            }
        }
    }
}