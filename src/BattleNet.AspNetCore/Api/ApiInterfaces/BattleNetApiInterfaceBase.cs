using BattleNetApi.Api.Enums;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BattleNetApi.Api.ApiInterfaces
{
    public class BattleNetApiInterfaceBase
    {
        private const string _baseUriMissingRegionAndEndpoint = "https://{0}.api.battle.net/";
        protected Region _region { get; set; }
        protected Locale _locale { get; set; }

        protected string RegionString
        {
            get
            {
                return _region.ToString().ToLower();
            }
        }
        protected string LocaleString
        {
            get
            {
                return _locale.ToString();
            }
        }

        protected string BaseApiUriFormat
        {
            get
            {
                return string.Format(_baseUriMissingRegionAndEndpoint, RegionString) + "{0}";
            }
        }

        internal BattleNetApiInterfaceBase(Region region, Locale locale)
        {
            _region = region;
            _locale = locale;
        }

        protected void SetJsonAcceptHeader(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected async Task<JObject> ParseJsonResponseAsync(HttpResponseMessage response)
        {
            string jsonStr = await response.Content.ReadAsStringAsync();
            return JObject.Parse(jsonStr);
        }
    }
}
