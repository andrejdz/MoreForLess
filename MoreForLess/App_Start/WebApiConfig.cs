using System.Web.Http;
using System.Web.Http.Cors;
using MoreForLess.Filters;

namespace MoreForLess
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new ValidateModelStateFilter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Enabling CORS.
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
