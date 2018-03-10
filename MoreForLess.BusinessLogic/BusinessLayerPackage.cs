using MoreForLess.BusinessLogic.Adapters;
using MoreForLess.BusinessLogic.Adapters.Interfaces;
using MoreForLess.BusinessLogic.Services;
using MoreForLess.BusinessLogic.Services.Interfaces;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace MoreForLess.BusinessLogic
{
    /// <summary>
    /// Business layer of registering services in SimpleInjector in runtime.
    /// Uses contract <see cref="IPackage"/> for allow registering a set of services.
    /// </summary>
    public class BusinessLayerPackage : IPackage
    {
        /// <summary>
        /// Registers injections in WebApi layer with a help of <see cref="SimpleInjector.Packaging"/>.
        /// Registers types for instances using the scoped lifestyle.
        /// </summary>
        /// <param name="container">Container of types and instances.</param>
        public void RegisterServices(Container container)
        {
            container.Register<IGoodService, GoodService>(Lifestyle.Scoped);
            container.Register<ICategoryService, CategoryService>(Lifestyle.Scoped);
            container.Register<IAddGoodsService, AddGoodsService>(Lifestyle.Scoped);
            container.Register<ICommentService, CommentService>(Lifestyle.Scoped);
            container.Register<IScoreService, ScoreService>(Lifestyle.Scoped);

            container.Register<ISignedRequestCreatorService<SignedRequestAmazonCreatorService>, SignedRequestAmazonCreatorService>(Lifestyle.Scoped);
            container.Register<IStoreAdapter<AmazonAdapter>, AmazonAdapter>(Lifestyle.Scoped);
        }
    }
}