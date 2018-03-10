using System;
using System.Threading.Tasks;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Services.Interfaces
{
    /// <summary>
    ///     Contains method to manage scores of goods.
    /// </summary>
    public interface IScoreService
    {
        /// <summary>
        ///     Adds score to good and save to database.
        /// </summary>
        /// <param name="scoreDomainModel">
        ///     Value of score and id of good.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when good with id doesn't exist.
        ///     Throws when instance of <see cref="ScoreDomainModel"/> didn't pass validation.
        /// </exception>
        Task CreateScoreAsync(ScoreDomainModel scoreDomainModel);

        /// <summary>
        ///     Gets average value of scores.
        /// </summary>
        /// <param name="goodId">
        ///     Good's id.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when scores for good haven't been added to database.
        /// </exception>
        /// <returns>
        ///     Average value of scores.
        /// </returns>
        Task<double> GetAverageScoreAsync(int goodId);

        /// <summary>
        ///     Deletes scores of specified good.
        /// </summary>
        /// <param name="goodId">
        ///     Good's id.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when good with id doesn't exist.
        ///     Throws when scores for good haven't been added to database.
        /// </exception>
        Task DeleteScoresAsync(int goodId);
    }
}
