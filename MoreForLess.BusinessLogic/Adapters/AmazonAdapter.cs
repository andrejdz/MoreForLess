using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using MoreForLess.BusinessLogic.Adapters.Interfaces;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Models.Comparers;
using MoreForLess.BusinessLogic.Services;
using MoreForLess.BusinessLogic.Services.Interfaces;

namespace MoreForLess.BusinessLogic.Adapters
{
    /// <inheritdoc />
    public class AmazonAdapter : IStoreAdapter<AmazonAdapter>
    {
        private readonly ISignedRequestCreatorService<SignedRequestAmazonCreatorService> _signedRequestCreatorService;

        // Namespace of xml.
        private XNamespace _xns;

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

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<GoodDomainModel>> GetItemInfoByUrlAsync(RequestParametersModel requestParametersModel)
        {
            var signedRequest = this._signedRequestCreatorService.CreateSignedRequest(requestParametersModel);

            //await Task.Delay(1000);

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
        private IReadOnlyCollection<GoodDomainModel> Parse(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            // Creating xml element from string that contains xml data.
            var xmlData = XElement.Parse(data);

            // Getting default namespace of xml.
            this._xns = xmlData.GetDefaultNamespace();

            IEnumerable<XElement> itemCollection;
            try
            {
                itemCollection = xmlData.Element(this._xns + "Items")
                    .Elements(this._xns + "Item");
            }
            catch (NullReferenceException ex)
            {
                throw new ArgumentException("Error when getting good's info from xml.", ex);
            }

            List<GoodDomainModel> goodDomainModels = new List<GoodDomainModel>();
            foreach (var item in itemCollection)
            {
                IEnumerable<CategoryDomainModel> categoryDomainModels;
                try
                {
                    categoryDomainModels = this.GetCategories(item);
                }
                catch (NullReferenceException ex)
                {
                    throw new ArgumentException("Error when getting categories from xml.", ex);
                }


                // Getting necessary data from xml.
                string name;
                try
                {
                    name = item.Element(this._xns + "ItemAttributes")
                        .Element(this._xns + "Title")
                        .Value;
                }
                catch (NullReferenceException ex)
                {
                    throw new ArgumentException("Error when getting good's name from xml.", ex);
                }

                decimal price;
                try
                {
                    price = this.GetPrice(item);
                }
                catch (NullReferenceException ex)
                {
                    throw new ArgumentException("Error when getting good's price from xml.", ex);
                }

                if (price == decimal.MinusOne)
                {
                    continue;
                }

                string idGoodOnShop;
                try
                {
                    idGoodOnShop = item.Element(this._xns + "ASIN")
                        .Value;
                }
                catch (NullReferenceException ex)
                {
                    throw new ArgumentException("Error when getting good's id from xml.", ex);
                }

                string linkOnProduct;
                try
                {
                    linkOnProduct = item.Element(this._xns + "DetailPageURL")
                        .Value;
                }
                catch (NullReferenceException ex)
                {
                    throw new ArgumentException("Error when getting good's url from xml.", ex);
                }

                string linkOnPicture;
                try
                {
                    linkOnPicture = item.Element(this._xns + "MediumImage")
                        .Element(this._xns + "URL")
                        .Value;
                }
                catch (NullReferenceException)
                {
                    linkOnPicture = null;
                }

                string categoryIdOnShop;
                try
                {
                    categoryIdOnShop = item.Element(this._xns + "BrowseNodes")
                        .Elements(this._xns + "BrowseNode")
                        .First()
                        .Element(this._xns + "BrowseNodeId")
                        .Value;
                }
                catch (NullReferenceException ex)
                {
                    throw new ArgumentException("Error when getting good's category from xml.", ex);
                }

                GoodDomainModel goodDomainModel = new GoodDomainModel
                {
                    Name = name,
                    Price = price,
                    IdGoodOnShop = idGoodOnShop,
                    LinkOnProduct = linkOnProduct,
                    LinkOnPicture = linkOnPicture,
                    CurrencyName = "USD",
                    ShopName = "AMAZON",
                    CategoryIdOnShop = categoryIdOnShop,
                    CategoryIdsOnShop = categoryDomainModels
                };

                goodDomainModels.Add(goodDomainModel);
            }

            return goodDomainModels;
        }

        private decimal GetPrice(XElement item)
        {
            bool lowestPriceExist;
            try
            {
                lowestPriceExist = item.Element(this._xns + "OfferSummary")
                    .Elements()
                    .Any(i => i.Name == this._xns + "LowestNewPrice");
            }
            catch (NullReferenceException)
            {
                // Node OfferSummary doesn't exist.
                lowestPriceExist = false;
            }

            bool listPriceExist;
            try
            {
                listPriceExist = item.Element(this._xns + "ItemAttributes")
                    .Elements()
                    .Any(i => i.Name == this._xns + "ListPrice");
            }
            catch (NullReferenceException)
            {
                // Node ItemAttributes doesn't exist.
                listPriceExist = false;
            }

            string priceString;
            if (lowestPriceExist)
            {
                priceString = item.Element(this._xns + "OfferSummary")
                    .Element(this._xns + "LowestNewPrice")
                    .Element(this._xns + "FormattedPrice")
                    .Value;
            }
            else if (listPriceExist)
            {
                priceString = item.Element(this._xns + "ItemAttributes")
                    .Element(this._xns + "ListPrice")
                    .Element(this._xns + "FormattedPrice")
                    .Value;
            }
            else
            {
                return decimal.MinusOne;
            }

            // Getting price without currency sign.
            var priceRegex = new Regex("[0-9,]+([.,][0-9]+)?", RegexOptions.CultureInvariant);
            var priceWithoutCurrencySign = priceRegex.Match(priceString).Value;

            // Replacing thousands comma separators by empty places.
            var commaRegex = new Regex(@"\,(?=\d{3})", RegexOptions.CultureInvariant);
            var priceWithoutCommas = commaRegex.Replace(priceWithoutCurrencySign, string.Empty);

            decimal price;
            try
            {
                price = Convert.ToDecimal(priceWithoutCommas, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return decimal.MinusOne;
            }

            return price;
        }

        private IEnumerable<CategoryDomainModel> GetCategories(XElement item)
        {
            // Getting categories of good.
            var categories = item.Element(this._xns + "BrowseNodes")
                .Elements(this._xns + "BrowseNode");

            List<CategoryDomainModel> categoryDomainModels = new List<CategoryDomainModel>();

            // Creating collection of CategoryDomainModel.
            foreach (var category in categories)
            {
                var childCategories = category.Element(this._xns + "Children")
                    ?.Elements(this._xns + "BrowseNode")
                    .Select(e => new CategoryDomainModel
                    {
                        IdAtStore = e.Element(this._xns + "BrowseNodeId").Value,
                        Name = e.Element(this._xns + "Name").Value,
                        ParentIdAtStore = category.Element(this._xns + "BrowseNodeId").Value
                    });

                if (childCategories != null)
                {
                    categoryDomainModels.AddRange(childCategories);
                }

                // Getting all categories related to current good.
                this.GetAncestorsCategories(categoryDomainModels, category);
            }

            return categoryDomainModels.Distinct(new CategoryEqualityComparer());
        }

        /// <summary>
        ///     Gets all ancestor's categories related to the current good.
        /// </summary>
        /// <param name="categoryDomainModels">
        ///     Collection of instances of <see cref="CategoryDomainModel"/>
        /// </param>
        /// <param name="category">
        ///     Category of good.
        /// </param>
        /// <returns>
        ///     Main category of good.
        /// </returns>
        private CategoryDomainModel GetAncestorsCategories(
            IList<CategoryDomainModel> categoryDomainModels,
            XElement category)
        {
            if (category?.Element(this._xns + "Ancestors") != null)
            {
                var parentCategory = category.Element(this._xns + "Ancestors")
                    .Element(this._xns + "BrowseNode");

                var parentCategoryDomainModel = this.GetAncestorsCategories(categoryDomainModels, parentCategory);

                var categoryDomainModel = new CategoryDomainModel
                {
                    IdAtStore = category.Element(this._xns + "BrowseNodeId").Value,
                    Name = category.Element(this._xns + "Name").Value,
                    ParentIdAtStore = parentCategoryDomainModel.IdAtStore
                };

                categoryDomainModels.Add(categoryDomainModel);

                return categoryDomainModel;
            }
            else
            {
                var categoryDomainModel = new CategoryDomainModel
                {
                    IdAtStore = category.Element(this._xns + "BrowseNodeId").Value,
                    Name = category.Element(this._xns + "Name") == null
                        ? "Other category"
                        : category.Element(this._xns + "Name").Value,
                    ParentIdAtStore = null
                };

                categoryDomainModels.Add(categoryDomainModel);

                return categoryDomainModel;
            }
        }
    }
}
