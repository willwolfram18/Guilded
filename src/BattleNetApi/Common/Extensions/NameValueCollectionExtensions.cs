using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace BattleNetApi.Common.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static string ToQueryString(this NameValueCollection collection)
        {
            List<string> queryParams = new List<string>();
            foreach (string queryParam in collection.AllKeys)
            {
                foreach (string queryValue in collection.GetValues(queryParam))
                {
                    queryParams.Add(
                        string.Format(
                            "{0}={1}", 
                            WebUtility.UrlEncode(queryParam), 
                            WebUtility.UrlEncode(queryValue)
                        )
                    );
                }
            }
            return string.Join("&", queryParams);
        }
    }
}