using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Services.Interfaces
{
    /// <summary>
    ///     Contains contract of method that should create signed request.
    /// </summary>
    /// <typeparam name="T">
    ///     Type of service using to create signed request.
    /// </typeparam>
    public interface ISignedRequestCreatorService<T>
        where T : class
    {
        /// <summary>
        ///     Creates the signed request with
        ///     specified parameters
        /// </summary>
        /// <param name="requestParametersModel">
        ///     Contains request parameters.
        /// </param>
        /// <returns>
        ///     Signed request.
        /// </returns>
        string CreateSignedRequest(RequestParametersModel requestParametersModel);
    }
}
