namespace MoreForLess.BusinessLogic.Models
{
    /// <summary>
    ///     Type that stores request parameters.
    /// </summary>
    public class RequestParametersModel
    {
        /// <summary>
        ///     Gets or sets page of xml document.
        /// </summary>
        public int Page { get; set; }


        /// <summary>
        ///     Gets or sets lower border of price.
        ///     For purpose of use as request parameter must be multiply by 100.
        /// </summary>
        public int MinPrice { get; set; }

        /// <summary>
        ///     Gets or sets upper border of price.
        ///     For purpose of use as request parameter must be multiply by 100.
        /// </summary>
        public int MaxPrice { get; set; }
    }
}
