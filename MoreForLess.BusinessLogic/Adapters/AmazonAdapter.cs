using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using MoreForLess.BusinessLogic.Adapters.Interfaces;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Services;
using MoreForLess.BusinessLogic.Services.Interfaces;

namespace MoreForLess.BusinessLogic.Adapters
{
    /// <summary>
    ///     Type that receive info from url
    ///     and retrieve container with necessary data.
    /// </summary>
    public class AmazonAdapter : IStoreAdapter
    {
        private readonly ISignedRequestCreatorService<SignedRequestAmazonCreatorService> _signedRequestCreatorService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AmazonAdapter"/> class.
        /// </summary>
        /// <param name="signedRequestCreatorService">
        ///     Creator for building a signed request to
        ///     Amazon's server.
        /// </param>
        public AmazonAdapter(ISignedRequestCreatorService<SignedRequestAmazonCreatorService> signedRequestCreatorService)
        {
            this._signedRequestCreatorService = signedRequestCreatorService;
        }

        /// <summary>
        ///     Retrieves item information by provided URL.
        /// </summary>
        /// <param name="urlInfo">
        ///     The URLInfo object that describes item location in store.
        /// </param>
        /// <returns>
        ///     The ItemInfo object that contains item information or null if information isn't found.
        /// </returns>
        /// <exception cref="HttpRequestException">
        ///     Throws when requested data doesn't exist on remote server.
        /// </exception>
        public async Task<ItemInfo> GetItemInfoByURLAsync(URLInfo urlInfo)
        {
            var signedRequest = this._signedRequestCreatorService.CreateSignedRequest(urlInfo.Id);

            string responseMessage;
            using (var client = new HttpClient())
            {
                responseMessage = await client.GetStringAsync(signedRequest);
            }

            return this.Parse(responseMessage);
        }

        /// <summary>
        ///     Parses xml format string.
        /// </summary>
        /// <param name="data">
        ///     Xml format string to parse.
        /// </param>
        /// <returns>
        ///     The ItemInfo object that contains parsed information or null if information isn't found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Throws when data is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Throws when cannot get good's info from xml.
        /// </exception>
        private ItemInfo Parse(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            // Creatting xml element from string that contains xml data.
            var xmlData = XElement.Parse(data);

            // Getting default namespace of xml.
            var xns = xmlData.GetDefaultNamespace();

            // Getting necessary data from xml.
            string name;
            try
            {
                name = xmlData.Descendants(xns + "ItemAttributes")
                              .Elements(xns + "Title")
                              .First()
                              .Value;
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Error when getting good's name from xml.", ex);
            }

            string imageUrl;
            try
            {
                imageUrl = xmlData.Descendants(xns + "MediumImage")
                                  .Elements(xns + "URL")
                                  .First()
                                  .Value;
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Error when getting good's url of image from xml.", ex);
            }

            decimal price;
            try
            {
                var priceString = xmlData.Descendants(xns + "LowestNewPrice")
                                         .Elements(xns + "FormattedPrice")
                                         .First()
                                         .Value;

                // Getting price without currency sign.
                var priceRegex = new Regex("[0-9,]+([.,][0-9]+)?", RegexOptions.CultureInvariant);
                var priceWithoutCurrencySign = priceRegex.Match(priceString).Value;

                // Replacing thouthands comma separators by empty places.
                var commaRegex = new Regex(@"\,(?=\d{3})", RegexOptions.CultureInvariant);
                var priceWithoutCommas = commaRegex.Replace(priceWithoutCurrencySign, string.Empty);

                price = Convert.ToDecimal(priceWithoutCommas, CultureInfo.InvariantCulture);
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Error when getting good's price from xml.", ex);
            }

            return new ItemInfo
            {
                Currency = "USD",
                ImageURL = imageUrl,
                Name = name,
                Price = price
            };
        }
    }
}
