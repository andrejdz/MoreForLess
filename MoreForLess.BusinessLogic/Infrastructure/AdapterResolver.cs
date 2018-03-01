using System;
using System.Collections.Generic;
using System.Linq;
using MoreForLess.BusinessLogic.Adapters;
using MoreForLess.BusinessLogic.Adapters.Interfaces;
using MoreForLess.BusinessLogic.Infrastructure.Interfaces;
using MoreForLess.BusinessLogic.Services;
using MoreForLess.BusinessLogic.Services.Interfaces;

namespace MoreForLess.BusinessLogic.Infrastructure
{
    /// <summary>
    ///     Uses to resolve what platform's adapter to use.
    /// </summary>
    public class AdapterResolver : IAdapterResolver
    {
        private readonly ISignedRequestCreatorService<SignedRequestAmazonCreatorService> _signedRequestCreatorService;

        // Collection of adapters.
        private readonly IReadOnlyDictionary<string, Type> _adapters = new Dictionary<string, Type>
        {
            { "Onliner", typeof(OnlinerAdapter) },
            { "AliExpress", typeof(AliExpressAdapter) },
            { "Amazon", typeof(AmazonAdapter) }
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="AdapterResolver"/> class.
        /// </summary>
        /// <param name="signedRequestCreatorService">
        ///     Creator for building a signed request to
        ///     Amazon's server.
        /// </param>
        public AdapterResolver(
            ISignedRequestCreatorService<SignedRequestAmazonCreatorService> signedRequestCreatorService)
        {
            this._signedRequestCreatorService = signedRequestCreatorService;
        }

        /// <summary>
        ///     Resolves what adapter to use.
        /// </summary>
        /// <param name="platform">
        ///     Name of platform.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when platform isn't supported
        /// </exception>
        /// <returns>
        ///     Necessary instance of adapter.
        /// </returns>
        public IStoreAdapter Resolve(string platform)
        {
            if (!this._adapters.ContainsKey(platform))
            {
                throw new ArgumentException($"Adapter for \"{platform}\" isn't supported.");
            }

            // Gets type of adapter by name of platform.
            var typeOfAdapter = this._adapters[platform];

            switch (this._adapters.First(p => p.Key == platform).Key)
            {
                case "Onliner":
                case "AliExpress":

                    // Creates instance of specified type of adapter with parameterless constructor.
                    return (IStoreAdapter)Activator.CreateInstance(typeOfAdapter);
                case "Amazon":
                case "eBay":

                    // Creates instance of specified type of adapter.
                    return (IStoreAdapter)Activator.CreateInstance(typeOfAdapter, this._signedRequestCreatorService);
                default:
                    throw new ArgumentException($"Adapter for \"{platform}\" isn't supported.");
            }
        }
    }
}