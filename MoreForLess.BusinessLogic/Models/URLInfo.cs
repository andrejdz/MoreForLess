namespace MoreForLess.BusinessLogic.Models
{
    /// <summary>
    ///     Represents a parsed information from item URL.
    /// </summary>
    public class URLInfo
    {
        /// <summary>
        ///     Gets or sets the platform of the item.
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        ///     Gets or sets the id of the item.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the absolute uri of the item.
        /// </summary>
        public string AbsoluteUri { get; set; }
    }
}