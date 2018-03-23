using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using FluentValidation;
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
        private readonly IValidator<PageCategoryModel> _pageCategoryModelValidator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GoodsController"/> class.
        /// </summary>
        /// <param name="goodService">
        ///     Service that manages goods.
        /// </param>
        /// <param name="addGoodsService">
        ///     Service that adds or updates goods.
        /// </param>
        /// <param name="pageCategoryModelValidator">
        ///     Validator for <see cref="PageCategoryModel"/> instances.
        /// </param>
        public GoodsController(
            IGoodService goodService,
            IAddGoodsService addGoodsService,
            IValidator<PageCategoryModel> pageCategoryModelValidator)
        {
            this._goodService = goodService;
            this._addGoodsService = addGoodsService;
            this._pageCategoryModelValidator = pageCategoryModelValidator;
        }

        /// <summary>
        ///     Gets collection of goods and paging information.
        /// </summary>
        /// <param name="currenPage">
        ///     Number of current page.
        /// </param>
        /// <param name="itemsPerPage">
        ///     Items per page.
        /// </param>
        /// <param name="categoryId">
        ///     Category id at store.
        /// </param>
        /// <returns>
        ///     Collection of goods and paging information.
        /// </returns>
        [Route("", Name = "GetAllGoods")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(IEnumerable<GoodPagingDomainModel>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Get (int currenPage, int itemsPerPage, string categoryId)
        {
            PageCategoryModel pageCategoryModel = new PageCategoryModel
            {
                CurrentPage = currenPage,
                ItemsPerPage = itemsPerPage,
                CategoryId = categoryId
            };

            try
            {
                // Validating instance of type PageCategoryModel.
                this._pageCategoryModelValidator.ValidateAndThrow(pageCategoryModel);
            }
            catch (ValidationException ex)
            {
                _logger.Info(ex, ex.ToString());
                return this.BadRequest();
            }

            _logger.Info("Getting all goods that are stored in database.");
            GoodPagingDomainModel goodPagingDomainModels;
            try
            {
                goodPagingDomainModels = await this._goodService.GetAllGoodsAsync(pageCategoryModel);
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
