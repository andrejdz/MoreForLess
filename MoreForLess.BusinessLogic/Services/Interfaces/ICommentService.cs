using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Services.Interfaces
{
    /// <summary>
    ///     Contains method to manage comments of goods.
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        ///     Adds comment to good and save to database.
        /// </summary>
        /// <param name="commentDomainModel">
        ///     Text of comment and id of good that comment belongs.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when good with id doesn't exist.
        ///     Throws when instance of <see cref="CommentDomainModel"/> didn't pass validation.
        /// </exception>
        Task CreateCommentAsync(CommentDomainModel commentDomainModel);

        /// <summary>
        ///     Gets all comments of specified good.
        /// </summary>
        /// <param name="goodId">
        ///     Good's id.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when comments for good haven't been added to database.
        /// </exception>
        /// <returns>
        ///     Collection of texts of comments.
        /// </returns>
        Task<IReadOnlyCollection<CommentDomainModel>> GetCommentsAsync(int goodId);

        /// <summary>
        ///     Deletes comments of specified good.
        /// </summary>
        /// <param name="goodId">
        ///     Good's id.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when good with id doesn't exist.
        ///     Throws when comments for good haven't been added to database.
        /// </exception>
        Task DeleteCommentsAsync(int goodId);
    }
}
