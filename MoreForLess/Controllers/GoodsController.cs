using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Services.Interfaces;
using Swashbuckle.Swagger.Annotations;

namespace MoreForLess.Controllers
{
    /// <inheritdoc />
    [RoutePrefix("api/goods")]
    public class GoodsController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IGoodService _goodService;
        private readonly IAddGoodsService _addGoodsService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GoodsController"/> class.
        /// </summary>
        /// <param name="goodService">
        ///     Service that manages goods.
        /// </param>
        /// <param name="addGoodsService">
        ///     Service that adds or updates goods.
        /// </param>
        public GoodsController(
            IGoodService goodService,
            IAddGoodsService addGoodsService)
        {
            this._goodService = goodService;
            this._addGoodsService = addGoodsService;
        }

        /// <summary>
        ///     Gets collection of goods and paging information.
        /// </summary>
        /// <param name="currentPage">
        ///     Number of current page.
        /// </param>
        /// <param name="itemsPerPage">
        ///     Number of items per page.
        /// </param>
        /// <returns>
        ///     Collection of goods and paging information.
        /// </returns>
        [Route("", Name = "GetAllGoods")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(IEnumerable<GoodPagingDomainModel>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Get(int currentPage, int itemsPerPage)
        {
            if (currentPage < 1 || itemsPerPage < 1)
            {
                string info = $"{nameof(currentPage)} or {nameof(itemsPerPage)} less than 1.";
                _logger.Info(info);
                return this.BadRequest(info);
            }

            _logger.Info("Getting all goods that are stored in database.");
            GoodPagingDomainModel goodPagingDomainModels;
            try
            {
                goodPagingDomainModels = await this._goodService.GetAllGoodsAsync(currentPage, itemsPerPage);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.ToString());
                return this.InternalServerError();
            }

            _logger.Info("All goods have successfully been retrieved.");
            return this.Ok(goodPagingDomainModels);
        }

        /// <summary>
        ///     Gets good by id.
        /// </summary>
        /// <param name="id">
        ///     Id of good.
        /// </param>
        /// <returns>
        ///     Good's data.
        /// </returns>
        [Route("{id}", Name = "GetGoodById")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(GoodDomainModel))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Get(int id)
        {
            _logger.Info($"Getting a good with id = {id} that is stored in database.");
            GoodDomainModel goodDomainModel;
            try
            {
                goodDomainModel = await this._goodService.GetGoodByIdAsync(id);
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

            _logger.Info($"Good: {goodDomainModel.Name} has successfully been retrieved.");
            return this.Ok(goodDomainModel);
        }

        /// <summary>
        ///     Start update good's data into database.
        /// </summary>
        /// <param name="minPrice">
        ///     Minimum price is multiplied by 100.
        /// </param>
        /// <returns>
        ///     Http result.
        /// </returns>
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Post(int minPrice)
        {
            _logger.Info($"Start process of adding good's data to database.");

            try
            {
                await this._addGoodsService.AddGoodsAsync(minPrice);
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

            string messageIfSuccess = $"Goods have been successfully added.";
            _logger.Info(messageIfSuccess);
            return this.Ok(messageIfSuccess);
        }

        /// <summary>
        ///     Deletes good by id.
        /// </summary>
        /// <param name="id">
        ///     Id of good.
        /// </param>
        /// <returns>
        ///     Http result.
        /// </returns>
        [Route("{id}")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Delete(int id)
        {
            _logger.Info($"Deleting a good with id = {id}.");
            try
            {
                await this._goodService.DeleteAsync(id);
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

            _logger.Info($"Good with id = {id} has been successfully deleted.");
            return this.StatusCode(HttpStatusCode.NoContent);
        }
    }
}
