using System.Collections.Generic;

namespace MoreForLess.BusinessLogic.Models
{
    public class GoodDomainModel
    {
        /// <summary>
        ///     Gets or sets good's in database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets a good's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a good's price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        ///     Gets or sets a good's id.
        ///     The value depends on a shop.
        /// </summary>
        public string IdGoodOnShop { get; set; }

        /// <summary>
        ///     Gets or sets a good's url.
        /// </summary>
        public string LinkOnProduct { get; set; }

        /// <summary>
        ///     Gets or sets a good's url of image.
        /// </summary>
        public string LinkOnPicture { get; set; }

        /// <summary>
        ///     Gets or sets a currency of a price.
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        ///     Gets or sets a name of store.
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        ///     Gets or sets an category of good at store.
        /// </summary>
        public string CategoryIdOnShop { get; set; }

        /// <summary>
        ///     Gets or sets categories related to this good.
        /// </summary>
        public IEnumerable<CategoryDomainModel> Categories { get; set; }
    }
}