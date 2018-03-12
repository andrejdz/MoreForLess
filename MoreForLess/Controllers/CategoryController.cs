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
    [RoutePrefix("api/categories")]
    public class CategoryController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ICategoryService _categoryService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="categoryService">
        ///     Service that manages categories.
        /// </param>
        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        /// <summary>
        ///     Gets collection of categories.
        /// </summary>
        /// <returns>
        ///     Collection of categories.
        /// </returns>
        [Route("", Name = "GetCategories")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(IEnumerable<CategoryDomainModel>))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Get()
        {
            _logger.Info($"Getting all categories.");
            IReadOnlyCollection<CategoryDomainModel> categoryDomainModels;
            try
            {
                categoryDomainModels = await this._categoryService.GetAllCategoriesAsync();
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

            _logger.Info("All categories have successfully been retrieved.");
            return this.Ok(categoryDomainModels);
        }

        /// <summary>
        ///     Gets category by its id.
        /// </summary>
        /// <returns>
        ///     Category.
        /// </returns>
        [Route("{id}", Name = "GetCategory")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(CategoryDomainModel))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Get(string id)
        {
            _logger.Info($"Getting category with id: {id}.");
            CategoryDomainModel categoryDomainModel;
            try
            {
                categoryDomainModel = await this._categoryService.GetCategoryByIdAsync(id);
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

            _logger.Info($"Category with id: {id} has successfully been retrieved.");
            return this.Ok(categoryDomainModel);
        }

        /// <summary>
        ///     Deletes category by its id.
        /// </summary>
        /// <param name="id">
        ///     Id of category.
        /// </param>
        /// <returns>
        ///     Http result.
        /// </returns>
        [Route("{id}")]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Delete(string id)
        {
            _logger.Info($"Deleting category with id: {id}.");
            try
            {
                await this._categoryService.DeleteCategoryAsync(id);
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

            _logger.Info($"Category with id: {id} has been successfully deleted.");
            return this.StatusCode(HttpStatusCode.NoContent);
        }
    }
}
