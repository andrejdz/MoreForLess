using System;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsQuery;
using MoreForLess.BusinessLogic.Adapters.Interfaces;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Adapters
{
    public class OnlinerAdapter : IStoreAdapter
    {
        /// <summary>
        ///     Compiled regular expression for finding low price.
        /// </summary>
        private readonly Regex _priceRegex = new Regex(
            "[0-9]+([,.][0-9]+)*",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        ///     Retrieves item information by provided URL.
        /// </summary>
        /// <param name="urlInfo">
        ///     The URLInfo object that describes item location in store.
        /// </param>
        /// <returns>
        ///     The ItemInfo object that contains item information or null if information isn't found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Throws when urlInfo is null.
        /// </exception>
        /// <exception cref="HttpRequestException">
        ///     Throws when requested data doesn't exist on remote server.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Throws when urlInfo.AbsoluteUri is null.
        /// </exception>
        public async Task<ItemInfo> GetItemInfoByURLAsync(URLInfo urlInfo)
        {
            string responseMessage;

            using (var client = new HttpClient())
            {
                responseMessage = await client.GetStringAsync(urlInfo.AbsoluteUri);
            }

            return this.Parse(responseMessage);
        }

        /// <summary>
        ///     Parsing provided html markup.
        /// </summary>
        /// <param name="data">
        ///     Html markup to parse.
        /// </param>
        /// <returns>
        ///     The ItemInfo object that contains parsed information or null if information isn't found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Throws when data is null.
        /// </exception>
        private ItemInfo Parse(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var csQuery = CQ.CreateDocument(data);
            var priceTag = csQuery[".offers-description__link_nodecor:first"].FirstElement();
            var imageTag = csQuery["img[id='device-header-image']:first"].FirstElement();
            var nameTag = csQuery["h1[itemprop='name']:first"].FirstElement();

            if (priceTag == null || priceTag.InnerText.Length < 1)
            {
                return null;
            }

            Match match = this._priceRegex.Match(priceTag.InnerText);

            if (!match.Success)
            {
                return null;
            }

            var priceString = match.Value.Replace(',', '.');

            if (!decimal.TryParse(priceString, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal price)
                || price <= 0)
            {
                return null;
            }

            var imageUrl = imageTag?["src"];

            // Minimum url length (11) for http://x.xx
            if (imageUrl == null || imageUrl.Length < 11)
            {
                return null;
            }

            if (nameTag == null || nameTag.InnerText.Length < 1)
            {
                return null;
            }

            return new ItemInfo
            {
                Currency = "BYN",
                ImageURL = imageUrl,
                Name = nameTag.InnerText.Trim(),
                Price = price
            };
        }
    }
}