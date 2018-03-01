using System;
using System.Threading.Tasks;
using NLog;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Services.Interfaces;

namespace MoreForLess.BusinessLogic.Services
{
    /// <summary>
    ///     Type contains method that allow adding good to database.
    /// </summary>
    public class AddingGoodService : IAddingGoodService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IGoodDomainModelCreatorService _goodDomainModelCreatorService;
        private readonly IGoodService _goodService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddingGoodService"/> class.
        /// </summary>
        /// <param name="goodDomainModelCreatorService">
        ///     Creator of instance pf type <see cref="GoodDomainModel"/>.
        /// </param>
        /// <param name="goodService">
        ///     Service that implements CRUD operations.
        /// </param>
        public AddingGoodService(
            IGoodDomainModelCreatorService goodDomainModelCreatorService,
            IGoodService goodService)
        {
            this._goodDomainModelCreatorService = goodDomainModelCreatorService;
            this._goodService = goodService;
        }

        /// <summary>
        ///     Adds good item to database.
        /// </summary>
        /// <param name="url">
        ///     Url of good on store's site.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when good's url is null, empty or
        ///     contains only white-space characters.
        /// </exception>
        /// <returns>
        ///     Good item with id defined by database.
        /// </returns>
        public async Task<GoodDomainModel> ProcessAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("Good's url is null, empty or contains only white-space characters.", nameof(url));
            }

            _logger.Info($"Start of parsing url process for: {url}.");
            GoodDomainModel goodDomainModel = await this._goodDomainModelCreatorService
                .CreateGoodDomainModelFromUrlAsync(url);

            _logger.Info($"Creating a good: {goodDomainModel.Name}.");

            return await this._goodService.CreateAsync(goodDomainModel);
        }
    }
}