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
        public async Task CreateAsync(IReadOnlyCollection<GoodDomainModel> goodDomainModels)
        {
            var goodDomainModelFirst = goodDomainModels.First();

            _logger.Info($"Retrieving currency id of {goodDomainModelFirst.CurrencyName}.");
            int currencyId;
            try
            {
                var currency = await this._context.Currencies
                    .SingleAsync(u => u.Name == goodDomainModelFirst.CurrencyName);

                currencyId = currency.Id;
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Error when getting currency's id", ex);
            }

            _logger.Info($"Retrieving shop id of {goodDomainModelFirst.ShopName}.");
            int shopId;
            try
            {
                var shop = await this._context.Shops
                    .SingleAsync(u => u.Name == goodDomainModelFirst.ShopName);

                shopId = shop.Id;
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Error when getting shop's id.", ex);
            }

            List<Good> goods = new List<Good>();
            foreach (var goodDomainModel in goodDomainModels)
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

                good.CurrencyId = currencyId;
                good.ShopId = shopId;

                _logger.Info($"Adding good item: {good.Name} to goods collection.");
                goods.Add(good);
            }

            _logger.Info($"Saving goods item into database.");
            this._context.Goods.AddRange(goods);

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
        public async Task<GoodPagingDomainModel> GetAllGoodsAsync(int currentPage, int itemsPerPage)
        {
            _logger.Info("Counting number of goods has been placed into database.");
            var totalItems = await this._context.Goods.CountAsync();

            _logger.Info("Retrieving collection of good items.");
            var goods = await this._context.Goods
                .OrderBy(g => g.Id)
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();

            var goodDomainModels = goods.Select(good => this._mapper.Map<GoodDomainModel>(good));

            var pagingDomainModel = new PagingDomainModel
            {
                CurrentPage = currentPage,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems
            };

            return new GoodPagingDomainModel
            {
                Goods = goodDomainModels,
                PageInfo = pagingDomainModel
            };
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