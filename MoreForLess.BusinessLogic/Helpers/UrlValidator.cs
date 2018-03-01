using System;

namespace MoreForLess.BusinessLogic.Helpers
{
    public static class UrlValidator
    {
        /// <summary>
        ///     Checks url.
        /// </summary>
        /// <param name="url">Url that will be checked.</param>
        /// <returns>
        ///     True if url is valid.
        ///     False if url is invalid.
        /// </returns>
        public static bool CheckUrl(string url)
        {
            return UrlValidator.CheckUrl(url, out var _);
        }

        public static bool CheckUrl(string url, out Uri uriResult)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}