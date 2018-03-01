using FluentValidation.WebApi;
using Newtonsoft.Json.Serialization;
using MoreForLess.Infrastructure;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace MoreForLess
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class Global : HttpApplication
    {
        private Container _container;

        public void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Changes pascal case property name to camel case.
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Initializes Injector configs.
            this._container = InjectorConfig.Initialize(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(this._container);

            // Adds instance of type FluentValidationFactory in to the ModelValidatorProviders collection.
            FluentValidationModelValidatorProvider.Configure(
                GlobalConfiguration.Configuration,
                provider => provider.ValidatorFactory = this._container.GetInstance<FluentValidationFactory>());
        }
    }
}