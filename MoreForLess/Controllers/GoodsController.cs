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
    [RoutePrefix("api/goods")]
    public class GoodsController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IGoodService _goodService;
        private readonly IAddGoodsService _addUpdateGoodsService;

        public GoodsController(
            IGoodService goodService,
            IAddGoodsService addUpdateGoodsService)
        {
            this._goodService = goodService;
            this._addUpdateGoodsService = addUpdateGoodsService;
        }

        /// <summary>
        ///     Gets list of goods.
        /// </summary>
        /// <returns>
        ///     Collection of goods.
        /// </returns>
        [Route("", Name = "GetAllGoods")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(IEnumerable<GoodDomainModel>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Get()
        {
            _logger.Info("Getting all goods that are stored in database.");
            IEnumerable<GoodDomainModel> goodList;
            try
            {
                goodList = await this._goodService.GetAllGoodsAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.ToString());
                return this.InternalServerError();
            }

            _logger.Info("All goods have successfully been retrieved.");
            return this.Ok(goodList);
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
        /// <param name="request">
        ///     Url of good.
        /// </param>
        /// <param name="page"></param>
        /// <returns>
        ///     Http result.
        /// </returns>
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Post()
        {
            _logger.Info($"Start process of adding good's data to database.");

            try
            {
                await this._addUpdateGoodsService.AddGoodsAsync();
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
