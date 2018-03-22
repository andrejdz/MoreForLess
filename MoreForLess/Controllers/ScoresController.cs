using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Services.Interfaces;
using NLog;
using Swashbuckle.Swagger.Annotations;

namespace MoreForLess.Controllers
{
    /// <inheritdoc />
    [RoutePrefix("api/scores")]
    public class ScoresController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IScoreService _scoreService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScoresController"/> class.
        /// </summary>
        /// <param name="scoreService">
        ///     Service that manages scores.
        /// </param>
        public ScoresController(IScoreService scoreService)
        {
            this._scoreService = scoreService;
        }

        /// <summary>
        ///     Gets average value of scores.
        /// </summary>
        /// <returns>
        ///     Average value of score.
        /// </returns>
        [Route("{goodId}", Name = "GetAverageScore")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(double))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Get(int goodId)
        {
            _logger.Info($"Getting average value of scores of good with id: {goodId}.");
            double averageValue;
            try
            {
                averageValue = await this._scoreService.GetAverageScoreAsync(goodId);
            }
            catch (ArgumentException ex)
            {
                _logger.Info(ex, ex.ToString());
                return this.NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.ToString());
                return this.InternalServerError();
            }

            _logger.Info("Average value of scores have successfully been retrieved.");
            return this.Ok(averageValue);
        }

        /// <summary>
        ///     Creates score and adds to specified good.
        /// </summary>
        /// <param name="scoreDomainModel">
        ///     Value of score and good's id that score belongs.
        /// </param>
        /// <returns>
        ///     Http result.
        /// </returns>
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(ScoreDomainModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Post(ScoreDomainModel scoreDomainModel)
        {
            _logger.Info($"Start process of adding score to database.");

            try
            {
                await this._scoreService.CreateScoreAsync(scoreDomainModel);
            }
            catch (ArgumentException ex)
            {
                _logger.Info(ex, ex.ToString());
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.ToString());
                return this.InternalServerError();
            }

            string messageIfSuccess = $"Score has been successfully added.";
            _logger.Info(messageIfSuccess);
            return this.Ok(scoreDomainModel);
        }

        /// <summary>
        ///     Deletes all scores of specified good.
        /// </summary>
        /// <param name="goodId">
        ///     Id of good.
        /// </param>
        /// <returns>
        ///     Http result.
        /// </returns>
        [Route("{goodId}")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Delete(int goodId)
        {
            _logger.Info($"Deleting all scores of good with id: {goodId}.");
            try
            {
                await this._scoreService.DeleteScoresAsync(goodId);
            }
            catch (ArgumentException ex)
            {
                _logger.Info(ex, ex.ToString());
                return this.NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.ToString());
                return this.InternalServerError();
            }

            _logger.Info($"All scores of good with id: {goodId} has been successfully deleted.");
            return this.StatusCode(HttpStatusCode.NoContent);
        }
    }
}
