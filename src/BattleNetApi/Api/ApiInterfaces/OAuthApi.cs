using BattleNetApi.Api.Enums;
using BattleNetApi.Objects.WoW;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace BattleNetApi.Api.ApiInterfaces
{
    public class OAuthApi : BattleNetApiInterfaceBase
    {
        #region Constructors
        public OAuthApi(Region region, Locale locale) : base(region, locale) { }
        #endregion

        #region Public interface
        public async Task<IEnumerable<Character>> WowProfileAsync(string accessToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                SetJsonAcceptHeader(httpClient);

                var response = await httpClient.GetAsync(WowOAuthProfileUri(accessToken).ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject profile = await ParseJsonResponseAsync(response);
                return ParseWowCharacterProfile(profile["characters"].AsJEnumerable());
            }
        }
        #endregion

        #region Private/internal interface
        private UriBuilder WowOAuthProfileUri(string accessToken)
        {
            string oAuthProfileUri = string.Format(BaseApiUriFormat, "wow/user/characters");
            UriBuilder uriBuilder = new UriBuilder(oAuthProfileUri);
            var query = QueryHelpers.ParseQuery(uriBuilder.Query);
            query["access_token"] = accessToken;
            uriBuilder.Query = query.ToString();
            return uriBuilder;
        }

        private List<Character> ParseWowCharacterProfile(IJEnumerable<JToken> wowCharactersJson)
        {
            List<Character> characters = new List<Character>();
            foreach (JObject characterJson in wowCharactersJson)
            {
                characters.Add(Character.BuildOAuthProfileCharacter(characterJson));
            }
            return characters;
        }
        #endregion
    }
}
