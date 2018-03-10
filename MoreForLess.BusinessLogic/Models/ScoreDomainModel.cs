namespace MoreForLess.BusinessLogic.Models
{
    /// <summary>
    ///     Contains good's id and its score.
    /// </summary>
    public class ScoreDomainModel
    {
        /// <summary>
        ///     Get or sets good's id.
        /// </summary>
        public int GoodId { get; set; }

        /// <summary>
        ///     Gets or sets score of specified good.
        /// </summary>
        public int Value { get; set; }
    }
}
