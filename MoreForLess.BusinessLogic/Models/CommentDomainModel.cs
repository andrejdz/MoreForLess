namespace MoreForLess.BusinessLogic.Models
{
    /// <summary>
    ///     Contains good's id and its comment.
    /// </summary>
    public class CommentDomainModel
    {
        /// <summary>
        ///     Get or sets good's id.
        /// </summary>
        public int GoodId { get; set; }

        /// <summary>
        ///     Gets or sets text of comment of specified good.
        /// </summary>
        public string Text { get; set; }
    }
}
