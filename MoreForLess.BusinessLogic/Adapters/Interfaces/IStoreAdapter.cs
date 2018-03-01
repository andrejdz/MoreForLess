using System.Threading.Tasks;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Adapters.Interfaces
{
    /// <summary>
    ///     Defines methods for getting data from store.
    /// </summary>
    public interface IStoreAdapter
    {
        /// <summary>
        ///     Retrieves item information by provided URLInfo object.
        /// </summary>
        /// <param name="urlInfo">
        ///     The URLInfo object that describes item location in store.
        /// </param>
        /// <returns>
        ///     The ItemInfo object that contains item information or null if information isn't found.
        /// </returns>
        Task<ItemInfo> GetItemInfoByURLAsync(URLInfo urlInfo);
    }
}