using BattleNetApi.Apis.Enums;
using BattleNetApi.Apis.Interfaces;

namespace BattleNetApi.Apis
{
    public class BattleNetApiClient
    {
        private string _apiClientKey { get; set; }
        private Region _region { get; set; }
        private Locale _locale { get; set; }

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

        public IOAuthApiMethods OAuthApi { get; private set; }

        public IWowCommunityApiMethods WowCommunityApi { get; private set; }

        public BattleNetApiClient(string apiClientKey, Region region = Region.US, Locale locale = Locale.en_US)
        {
            _apiClientKey = apiClientKey;
            _region = region;
            _locale = locale;

            InitializeApiInterfaces();
        }

        private void InitializeApiInterfaces()
        {
            OAuthApi = new OAuthApiMethods(_region, _locale);
            WowCommunityApi = new WowCommunityApiMethods(_apiClientKey, _region, _locale);
        }
    }
}
