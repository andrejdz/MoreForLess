using System;
using System.Threading.Tasks;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Services.Interfaces
{
    public interface IGoodDomainModelCreatorService
    {
        /// <summary>
        ///     Creates instance of type <see cref="GoodDomainModel"/>
        ///     from the good's url.
        /// </summary>
        /// <param name="url">
        ///     Good's url.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws when good already exists.
        ///     Throws when information at store isn't found.
        ///     Throws when platform doesn't supported.
        /// </exception>
        /// <exception cref="Exception">
        ///     Throws when unexpected error occurs.
        /// </exception>
        /// <returns>
        ///     Instance of <see cref="GoodDomainModel"/> or
        ///     null if there exists the same good.
        /// </returns>
        Task<GoodDomainModel> CreateGoodDomainModelFromUrlAsync(string url);
    }
}
