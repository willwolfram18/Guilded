using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SelamaApi.Extensions
{
    public static class JObjectExtensions
    {
        public static bool ContainsKey(this JObject json, string key)
        {
            IDictionary<string, JToken> jsonDict = json as IDictionary<string, JToken>;
            return jsonDict.ContainsKey(key);
        }
    }
}