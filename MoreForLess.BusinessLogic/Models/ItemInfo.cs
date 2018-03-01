namespace MoreForLess.BusinessLogic.Models
{
    /// <summary>
    ///     Represents parsed item information.
    /// </summary>
    public class ItemInfo
    {
        /// <summary>
        ///     Gets or sets item name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets item price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        ///     Gets or sets price currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        ///     Gets or sets item image URL.
        /// </summary>
        public string ImageURL { get; set; }
    }
}