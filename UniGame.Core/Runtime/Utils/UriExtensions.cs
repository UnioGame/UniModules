namespace UniModules.UniGame.Core.Runtime.Utils
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Text;

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
    }
}