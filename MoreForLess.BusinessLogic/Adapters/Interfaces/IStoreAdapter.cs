using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Adapters.Interfaces
{
    /// <summary>
    ///     Provides opportunity of getting collection of goods.
    /// </summary>
    public interface IStoreAdapter<T>
        where T : class
    {
        /// <summary>
        ///     Retrieves collection of goods.
        /// </summary>
        /// <param name="requestParametersModel">
        ///     Contains request parameters.
        /// </param>
        /// <returns>
        ///     Collection of goods.
        /// </returns>
        /// <exception cref="HttpRequestException">
        ///     Throws when requested data doesn't exist on remote server.
        /// </exception>
        Task<IReadOnlyCollection<GoodDomainModel>> GetItemInfoByUrlAsync(RequestParametersModel requestParametersModel);
    }
}