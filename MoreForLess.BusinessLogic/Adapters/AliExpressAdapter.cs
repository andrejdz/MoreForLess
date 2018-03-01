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
    /// <summary>
    /// AliExpress parser that's parse AliExpress HTML markup.
    /// </summary>
    public class AliExpressAdapter : IStoreAdapter
    {
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
        /// Parses HTML markup
        /// </summary>
        /// <param name="data">AliExpress HTML markup to parse</param>
        /// <returns>ItemInfo with product filled image,name,currency and price </returns>
        /// <exception cref="ArgumentException">
        ///     Throws when unable to parse product name from html markup.
        ///     Throws when unable to parse product currency from html markup
        ///     Throws when unable to parse product image url from html markup
        ///     Throws when unable to parse product price from html markup
        /// </exception>
        private ItemInfo Parse(string data)
        {
            this.ValidateData(data);

            var csQuery = CQ.CreateDocument(data);

            var nameDomElement = csQuery["h1[class='product-name']"].FirstElement()
                                 ?? throw new ArgumentException("Unable to parse product name from html markup");
            var currencyDomElement = csQuery["span[class='p-symbol']"].FirstElement()
                                 ?? throw new ArgumentException("Unable to parse product currency from html markup");
            var imageUrlDomElement = csQuery["a[class='ui-image-viewer-image-frame']"]["img"].FirstElement()
                                 ?? throw new ArgumentException("Unable to parse product image url from html markup");
            var priceDomElement = csQuery["span[class='p-price']"].FirstElement()
                                  ?? csQuery["span[itemprop='lowPrice']"].FirstElement();
            if (priceDomElement == null)
            {
                throw new ArgumentException("Unable to parse product price from html markup");
            }

            var itemInfo = new ItemInfo();
            itemInfo.Name = nameDomElement.InnerText;
            string tempPriceString = Regex.Replace(priceDomElement.InnerText, "[ &nbsp]+", string.Empty);
            tempPriceString = Regex.Replace(tempPriceString, "[,]+", ".");
            decimal.TryParse(tempPriceString, NumberStyles.Currency, CultureInfo.InvariantCulture, out var price);
            itemInfo.Price = price;
            itemInfo.Currency = currencyDomElement.GetAttribute("content");
            itemInfo.ImageURL = imageUrlDomElement.GetAttribute("src");

            return itemInfo;
        }

        /// <summary>
        /// Validates input data string with HTML markup
        /// </summary>
        /// <param name="data">Data to validate</param>
        /// <exception cref="ArgumentNullException">
        ///     Throws when data is null
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Throws when data string is empty
        /// </exception>
        private void ValidateData(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data == string.Empty)
            {
                throw new ArgumentException("HTML markup is empty");
            }
        }
    }
}