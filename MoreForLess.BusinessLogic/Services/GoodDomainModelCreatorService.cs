using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using MoreForLess.BusinessLogic.Adapters.Interfaces;
using MoreForLess.BusinessLogic.Infrastructure.Interfaces;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Services.Interfaces;

namespace MoreForLess.BusinessLogic.Services
{
    /// <summary>
    ///     Type is in charge of creating instance of
    ///     <see cref="GoodDomainModel"/> from the good's url.
    /// </summary>
    public class GoodDomainModelCreatorService : IGoodDomainModelCreatorService
    {
        private readonly IGoodService _goodService;
        private readonly IMapper _mapper;
        private readonly IURLParser _urlParser;
        private readonly IAdapterResolver _adapterResolver;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GoodDomainModelCreatorService"/> class.
        /// </summary>
        /// <param name="goodService">
        ///     Instance of type that implements interface
        ///     <see cref="IGoodService"/>.
        /// </param>
        /// <param name="mapper">
        ///     Instance of type that implements interface
        ///     <see cref="IMapper"/>.
        /// </param>
        /// <param name="urlParser">
        ///     Url parser.
        /// </param>
        /// <param name="adapterResolver">
        ///     Resolver for platform's adapter.
        /// </param>
        public GoodDomainModelCreatorService(
            IGoodService goodService,
            IMapper mapper,
            IURLParser urlParser,
            IAdapterResolver adapterResolver)
        {
            this._goodService = goodService;
            this._mapper = mapper;
            this._urlParser = urlParser;
            this._adapterResolver = adapterResolver;
        }

        /// <summary>
        ///     Creates instance of type <see cref="GoodDomainModel"/>
        ///     from the good's url.
        /// </summary>
        /// <param name="url">
        ///     Good's url.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when good already exists.
        ///     Throws when information at store isn't found.
        ///     Throws when platform doesn't supported.
        /// </exception>
        /// <returns>
        ///     Instance of <see cref="GoodDomainModel"/> or
        ///     null if there exists the same good.
        /// </returns>
        public async Task<GoodDomainModel> CreateGoodDomainModelFromUrlAsync(string url)
        {
            var urlInfo = this._urlParser.Parse(url);

            // Checks that an item exists in database.
            if (await this._goodService.IsItemExistAsync(urlInfo.AbsoluteUri))
            {
                throw new ArgumentException($"Good with typed url \"{urlInfo.AbsoluteUri}\" has already been added.");
            }

            IStoreAdapter adapter;

            try
            {
                // Gets platform's adapter.
                adapter = this._adapterResolver.Resolve(urlInfo.Platform);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Error when resolving adapter of platform: {urlInfo.Platform}", ex);
            }

            ItemInfo itemInfo;

            try
            {
                itemInfo = await adapter.GetItemInfoByURLAsync(urlInfo);
            }
            catch (HttpRequestException ex)
            {
                throw new ArgumentException($"Error when downloading html markup from shop's site: {urlInfo.Platform}.", ex);
            }

            if (itemInfo == null)
            {
                throw new ArgumentException("Failed to parse html markup.");
            }

            var goodDomainModel = this._mapper.Map<GoodDomainModel>(urlInfo);

            goodDomainModel = this._mapper.Map(itemInfo, goodDomainModel);
            return goodDomainModel;
        }
    }
}