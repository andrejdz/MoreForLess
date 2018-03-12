using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MoreForLess.Filters
{
    /// <summary>
    ///     Filter for validation errors.
    /// </summary>
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        /// <summary>
        ///     Filter return a new error response if the submitted request has validation errors.
        /// </summary>
        /// <param name="actionContext">
        ///     The HttpActionContext.
        /// </param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}
