using MoreForLess.DataAccess.EF;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace MoreForLess.DataAccess
{
    public class DataAccessPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            // DbContext must be unique for every request.
            container.Register<ApplicationDbContext>(Lifestyle.Scoped);
        }
    }
}