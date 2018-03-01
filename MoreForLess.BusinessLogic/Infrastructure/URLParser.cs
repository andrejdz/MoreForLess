using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MoreForLess.BusinessLogic.Helpers;
using MoreForLess.BusinessLogic.Infrastructure.Interfaces;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Infrastructure
{
    /// <summary>
    ///     Represents a url parser that gets platform and item id.
    /// </summary>
    public class URLParser : IURLParser
    {
        /// <summary>
        ///     The dictionary that contains a relation between host and platform information.
        /// </summary>
        private readonly Dictionary<string, PlatformInfo> _platforms = new Dictionary<string, PlatformInfo>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="URLParser"/> class.
        /// </summary>
        public URLParser()
        {
            var amazonPlatformInfo = new PlatformInfo
            {
                Platform = "Amazon",
                IdRegex = new Regex(@"(?<=\/)[A-Z0-9]{10}(?=\/)"),
                UrlRegex = new Regex(@"^(.+(?=\?)|.+$)")
            };

            this._platforms.Add("www.amazon.com", amazonPlatformInfo);
        }

        /// <summary>
        ///     Gets a platform and an item id from the url.
        /// </summary>
        /// <param name="url">
        ///     An url of the item.
        /// </param>
        /// <returns>
        ///     A URLInfo object that contains platform and item id.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Throws when url is null or empty.
        ///     Throws when provided platform isn't supported.
        ///     Throws when item id can't be found in URL.
        ///     Throws when platform url doesn't match
        /// </exception>
        /// <exception cref="FormatException">
        ///     Throws when url has invalid format.
        /// </exception>
        public URLInfo Parse(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url is null or empty.", nameof(url));
            }

            var isValid = UrlValidator.CheckUrl(url, out var result);

            if (!isValid)
            {
                throw new FormatException("url has invalid format.");
            }

            if (!this._platforms.ContainsKey(result.Host))
            {
                throw new ArgumentException($"Platform {result.Host} isn't supported.");
            }

            var info = this._platforms[result.Host];
            var matchId = info.IdRegex.Match(result.AbsolutePath);

            if (!matchId.Success)
            {
                throw new ArgumentException("Platform id not found.");
            }

            var matchUrl = info.UrlRegex.Match(result.AbsoluteUri);

            if (!matchId.Success)
            {
                throw new ArgumentException("Platform url doesn't match.");
            }

            return new URLInfo
            {
                AbsoluteUri = matchUrl.Value,
                Id = matchId.Value,
                Platform = info.Platform
            };
        }

        /// <summary>
        ///     Represents a platform information.
        /// </summary>
        private class PlatformInfo
        {
            /// <summary>
            ///     Gets or sets the platform of the item.
            /// </summary>
            public string Platform { get; set; }

            /// <summary>
            ///     Gets or sets regular expression for matching id from provided URL.
            /// </summary>
            public Regex IdRegex { get; set; }

            /// <summary>
            ///      Gets or sets regular expression for matching short url from provided URL.
            /// </summary>
            public Regex UrlRegex { get; set; }
        }
    }
}