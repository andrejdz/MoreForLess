using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using MoreForLess.BusinessLogic.Models;
using NLog;
using MoreForLess.BusinessLogic.Properties;
using MoreForLess.BusinessLogic.Services.Interfaces;

namespace MoreForLess.BusinessLogic.Services
{
    /// <inheritdoc />
    public class SignedRequestAmazonCreatorService : ISignedRequestCreatorService<SignedRequestAmazonCreatorService>
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public string CreateSignedRequest(RequestParametersModel requestParametersModel)
        {
            // Creating unsigned request string.
            var unsignedRequest = this.CreateUnsignedRequest(requestParametersModel);

            _logger.Info("Start creatting signed request.");

            // Endpoint for US server.
            const string endpoint = "http://webservices.amazon.com/onca/xml?";

            _logger.Info("Creatting request to sign.");
            var requestToSign = "GET\n" +
                                "webservices.amazon.com\n" +
                                "/onca/xml\n" +
                                unsignedRequest;

            _logger.Info("Getting AWSAccessKeyIdPrivate value from section: appSettings of web.config.");
            string awsAccessKeyIdPrivate = Settings.Default.AWSAccessKeyIdPrivate;

            _logger.Info($"Converting {nameof(awsAccessKeyIdPrivate)} and {nameof(requestToSign)} " +
                              $"to byte's array.");
            var secretKeyBytes = new ASCIIEncoding().GetBytes(awsAccessKeyIdPrivate);
            var unsignedRequestBytes = new ASCIIEncoding().GetBytes(requestToSign);

            _logger.Info("Encrypting string with secret key.");
            var signatureBytes = new HMACSHA256(secretKeyBytes)
                .ComputeHash(unsignedRequestBytes);

            // Recommendation from Amazon's helper page.
            var signature = Convert.ToBase64String(signatureBytes);

            var signatureEncodedLower = HttpUtility.UrlEncode(signature);

            _logger.Info("Replacing specific signs to percent-encoding.");
            var regex = new Regex(@"%[a-f0-9]{2}");
            var signatureEncoded = regex.Replace(signatureEncodedLower, n => n.Value.ToUpperInvariant());

            _logger.Info("Composing the signed request.");
            var signedRequest = endpoint +
                                unsignedRequest +
                                $"&Signature={signatureEncoded}";

            _logger.Info($"{nameof(signedRequest)} has been created successfully.");
            return signedRequest;
        }

        private string CreateUnsignedRequest(RequestParametersModel requestParametersModel)
        {
            _logger.Info("Start creatting unsigned request.");

            _logger.Info("Getting AWSAccessKeyIdPublic and AssociateTag values from section: appSettings of web.config.");
            string awsAccessKeyIdPublic = Settings.Default.AWSAccessKeyIdPublic;
            string associateTag = Settings.Default.AssociateTag;

            var timeStamp = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-ddT12:00:00.000Z");

            _logger.Info("Creatting collection of parameters using to compose request.");
            var parameters = new SortedDictionary<string, string>(new StringComparerByByte())
            {
                { "Service", "AWSECommerceService" },
                { "AWSAccessKeyId", $"{awsAccessKeyIdPublic}" },
                { "AssociateTag", $"{associateTag}" },
                { "Operation", "ItemSearch" },
                { "SearchIndex", "Electronics" },
                { "BrowseNode", "493964" }, // 172282 493964 2102313011
                { "ResponseGroup", "Images,ItemAttributes,OfferSummary,BrowseNodes" },
                { "Availability", "Available" },
                { "Condition", "New" },
                { "MinPercentageOff", "10" },
                { "MinimumPrice", $"{requestParametersModel.MinPrice}" },
                { "MaximumPrice", $"{requestParametersModel.MaxPrice}" },
                { "Sort", "price" },
                { "ItemPage", $"{requestParametersModel.Page}"},
                { "Timestamp", $"{timeStamp}" }
            };

            // Parameters with percent-encoding values. See RFC 3986 Section 2.1.
            var parametersEncoded = new Dictionary<string, string>();

            var regex = new Regex(@"%[a-f0-9]{2}");

            _logger.Info("Replacing specific signs to percent-encoding.");
            foreach (var p in parameters)
            {
                var key = p.Key;

                var valueLower = HttpUtility.UrlEncode(p.Value);
                var value = regex.Replace(valueLower, n => n.Value.ToUpperInvariant());

                parametersEncoded.Add(key, value);
            }

            var unsignedRequest = new StringBuilder();
            _logger.Info($"Creatting {nameof(unsignedRequest)} that contains key=value pairs that have been divided via sign &.");
            foreach (var p in parametersEncoded)
            {
                unsignedRequest.Append($"{p.Key}={p.Value}&");
            }

            _logger.Info($"{nameof(unsignedRequest)} has been created successfully.");
            return unsignedRequest.ToString().TrimEnd('&');
        }

        /// <summary>
        ///     Compares two byte arrays have been made by converting
        ///     strings to byte array.
        /// </summary>
        private class StringComparerByByte : Comparer<string>
        {
            public override int Compare(string first, string second)
            {
                // Determining the shortest string.
                var shortestString = first.Length > second.Length ? second.Length : first.Length;

                // Converting string to byte array.
                var encoding = new ASCIIEncoding();
                var firstInBytes = encoding.GetBytes(first);
                var secondInBytes = encoding.GetBytes(second);

                for (var i = 0; i < shortestString; i++)
                {
                    if (firstInBytes[i] != secondInBytes[i])
                    {
                        return firstInBytes[i].CompareTo(secondInBytes[i]);
                    }
                }

                if (first.Length != second.Length)
                {
                    return first.Length > second.Length ? 1 : -1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
