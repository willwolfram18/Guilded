using BattleNetApi.Apis.Enums;
using BattleNetApi.Apis.Interfaces;
using BattleNetApi.Objects.WoW;
using BattleNetApi.Objects.WoW.DataResources;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BattleNetApi.Apis
{
    public class WowCommunityApiMethods : BattleNetApiCollectionBase, IWowCommunityApiMethods
    {
        private string _apiClientKey { get; set; }

        private string ApiUriMissingEndpoint
        {
            get
            {
                return string.Format(BaseApiUriFormat, "wow/{0}");
            }
        }

        #region Constructors
        public WowCommunityApiMethods(string apiClientKey, Region region, Locale locale) : base(region, locale)
        {
            _apiClientKey = apiClientKey;
        }
        #endregion

        #region Public interface
        public async Task<Achievement> GetAchievementAsync(int id)
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(AchievementUri(id).ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject achievementJson = await ParseJsonResponseAsync(response);
                return Achievement.BuildFullAchievement(achievementJson);
            }
        }

        public async Task<Character> GetCharacterProfileAsync(string realm, string characterName, params string[] fields)
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(CharacterProfileUri(realm, characterName, fields).ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject characterJson = await ParseJsonResponseAsync(response);
                return Character.BuildCharacterProfileEndpoint(characterJson);
            }
        }

        public async Task<Guild> GetGuildProfileAsync(string realm, string guildName, params string[] fields)
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var getGuildProfileTask = httpClient.GetAsync(GuildProfileUri(realm, guildName, fields).ToString());
                var getGuildRealmTask = GetRealmStatusesAsync(realm);

                var realmListRepsonse = await getGuildRealmTask;
                var guildProfileResponse = await getGuildProfileTask;
                if (!guildProfileResponse.IsSuccessStatusCode || realmListRepsonse == null || realmListRepsonse.Count != 1)
                {
                    return null;
                }


                JObject guildJson = await ParseJsonResponseAsync(guildProfileResponse);
                return Guild.BuildGuildProfileFromJson(guildJson, realmListRepsonse[0]);
            }
        }

        public async Task<Item> GetItemAsync(int id)
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(ItemUri(id).ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject achievementJson = await ParseJsonResponseAsync(response);
                return Item.ParseItemJson(achievementJson);
            }
        }

        public async Task<List<RaceDataResource>> GetCharacterRacesAsync()
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(DataResourceUri("character/races").ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject racesJson = await ParseJsonResponseAsync(response);
                return RaceDataResource.BuildRacesList(racesJson);
            }
        }

        public async Task<List<ItemClassDataResource>> GetItemClassesAsync()
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(DataResourceUri("item/classes").ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject itemClassesJson = await ParseJsonResponseAsync(response);
                return ItemClassDataResource.BuildItemClassListFromJson(itemClassesJson);
            }
        }

        public async Task<List<RealmStatus>> GetRealmStatusesAsync(params string[] realms)
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(RealmStatusUri(realms).ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject realmStatusesJson = await ParseJsonResponseAsync(response);
                return RealmStatus.ParseStatusesJson(realmStatusesJson);
            }
        }
        #endregion

        #region Private/Internal functions
        private HttpClient BuildHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            SetJsonAcceptHeader(httpClient);
            return httpClient;
        }

        private UriBuilder AchievementUri(int id)
        {
            UriBuilder achievementUriBuilder = BuildUriWithEndpoint("achievement/" + id.ToString());
            achievementUriBuilder.Query = BuildCommonQuery().ToString();
            return achievementUriBuilder;
        }

        private UriBuilder ItemUri(int id)
        {
            UriBuilder itemUriBuilder = BuildUriWithEndpoint("item/" + id.ToString());
            itemUriBuilder.Query = BuildCommonQuery().ToString();
            return itemUriBuilder;
        }

        private UriBuilder CharacterProfileUri(string realmName, string characterName, params string[] fields)
        {
            UriBuilder characterProfileUriBuilder = BuildUriWithEndpoint(string.Format("character/{0}/{1}", realmName, characterName));
            var query = BuildCommonQuery();
            if (fields.Length > 0)
            {
                query["fields"] = string.Join(",", fields);
            }
            characterProfileUriBuilder.Query = query.ToString();

            return characterProfileUriBuilder;
        }

        private UriBuilder GuildProfileUri(string realm, string guildName, params string[] fields)
        {
            UriBuilder guildProfileUriBuilder = BuildUriWithEndpoint(string.Format("guild/{0}/{1}", realm, guildName));
            var query = BuildCommonQuery();
            if (fields.Length > 0)
            {
                query["fields"] = string.Join(",", fields);
            }
            guildProfileUriBuilder.Query = query.ToString();

            return guildProfileUriBuilder;
        }

        private UriBuilder DataResourceUri(string resourceEndPoint)
        {
            UriBuilder dataResourceUriBuilder = BuildUriWithEndpoint("data/" + resourceEndPoint);
            var query = BuildCommonQuery();
            dataResourceUriBuilder.Query = query.ToString();

            return dataResourceUriBuilder;
        }

        private UriBuilder RealmStatusUri(params string[] realms)
        {
            UriBuilder realmStatusUrilBuilder = BuildUriWithEndpoint("realm/status");
            var query = BuildCommonQuery();
            if (realms.Length > 0)
            {
                foreach (string realm in realms)
                {
                    query.Add("realm", realm);
                }
            }
            realmStatusUrilBuilder.Query = query.ToString();
            return realmStatusUrilBuilder;
        }

        private IDictionary<string, StringValues> BuildCommonQuery()
        {
            var query = QueryHelpers.ParseQuery("");
            query["locale"] = LocaleString;
            query["apikey"] = _apiClientKey;
            return query;
        }
        private UriBuilder BuildUriWithEndpoint(string endpoint)
        {
            return new UriBuilder(string.Format(ApiUriMissingEndpoint, endpoint));
        }
        #endregion
    }
}
