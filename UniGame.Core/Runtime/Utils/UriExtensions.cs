namespace UniModules.UniGame.Core.Runtime.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Text;
    using UnityEngine.Networking;

    public static class UriExtensions
    {
        public static Uri AddTimeStampToUri(this Uri address)
        {
            var uriBuilder = new UriBuilder(address);
            var query = new NameValueCollection();
            HttpUtility.ParseQueryString(uriBuilder.Query, Encoding.UTF8, query);
            query["x"] = DateTime.Now.ToFileTime().ToString(CultureInfo.InvariantCulture);
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }

        public static string CombineUrlParameters(this string url,IDictionary<string,string> parameters)
        {
            var urlParameters = new List<string>();
            foreach (var parameter in parameters) {
                var keyValue = $"{parameter.Key}={UnityWebRequest.EscapeURL(parameter.Value)}";
                urlParameters.Add(keyValue);
            }

            var targetUrl = url;
            if (urlParameters.Count > 0) {
                targetUrl += $"?{string.Join("&",urlParameters)}";
            }

            return targetUrl;
        }
    }
}