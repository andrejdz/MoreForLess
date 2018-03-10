using System;
using System.Collections.Generic;
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
    [RoutePrefix("api/comments")]
    public class CommentsController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ICommentService _commentService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommentsController"/> class.
        /// </summary>
        /// <param name="commentService">
        ///     Service that manages comments.
        /// </param>
        public CommentsController(ICommentService commentService)
        {
            this._commentService = commentService;
        }

        /// <summary>
        ///     Gets collection of comments.
        /// </summary>
        /// <returns>
        ///     Collection of comments.
        /// </returns>
        [Route("{goodId}", Name = "GetComments")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(IEnumerable<CommentDomainModel>))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Get(int goodId)
        {
            _logger.Info($"Getting all comments of good with id: {goodId}.");
            IReadOnlyCollection<CommentDomainModel> commentDomainModels;
            try
            {
                commentDomainModels = await this._commentService.GetCommentsAsync(goodId);
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

            _logger.Info("All good's comments have successfully been retrieved.");
            return this.Ok(commentDomainModels);
        }

        /// <summary>
        ///     Creates comment and adds to specified good.
        /// </summary>
        /// <param name="commentDomainModel">
        ///     Text of comment and good's id that comment belongs.
        /// </param>
        /// <returns>
        ///     Http result.
        /// </returns>
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Post(CommentDomainModel commentDomainModel)
        {
            _logger.Info($"Start process of adding comment to database.");

            try
            {
                await this._commentService.CreateCommentAsync(commentDomainModel);
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

            string messageIfSuccess = $"Comment has been successfully added.";
            _logger.Info(messageIfSuccess);
            return this.Ok(messageIfSuccess);
        }

        /// <summary>
        ///     Deletes all comments of specified good.
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
            _logger.Info($"Deleting all coomments of good with id: {goodId}.");
            try
            {
                await this._commentService.DeleteCommentsAsync(goodId);
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

            _logger.Info($"All comments of good with id: {goodId} has been successfully deleted.");
            return this.StatusCode(HttpStatusCode.NoContent);
        }
    }
}
