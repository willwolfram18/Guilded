using BattleNetApi.Api.Enums;
using BattleNetApi.Common;
using BattleNetApi.Objects.WoW.Enums;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BattleNetApi.Objects.WoW
{
    public class RealmStatus
    {
        #region Instance properties
        public RealmType Type { get; private set; }
        
        public RealmPopulation Population { get; private set; }
        
        public bool Queue { get; private set; }
        
        public bool Status { get; private set; }
        
        public string Name { get; private set; }
        
        public string Slug { get; private set; }
        
        public string BattleGroup { get; private set; }
        
        public Locale Locale { get; private set; }
        
        public string Timezone { get; private set; }
        
        public List<string> ConnectedRealms { get; private set; }
        #endregion

        internal static RealmStatus BuildRealmStatusWithOnlyName(string name)
        {
            return new RealmStatus(name);
        }

        internal static RealmStatus ParseRealmStatusJson(JObject realmStatusJson)
        {
            return new RealmStatus(realmStatusJson);
        }

        internal static List<RealmStatus> ParseStatusesJson(JObject realmStatusesJson)
        {
            List<RealmStatus> statuses = new List<RealmStatus>();
            foreach (var statusJson in realmStatusesJson["realms"].AsJEnumerable())
            {
                statuses.Add(ParseRealmStatusJson(statusJson.Value<JObject>()));
            }
            return statuses;
        }

        private RealmStatus(string realmName)
        {
            Name = realmName;
        }
        private RealmStatus(JObject realmStatusJson)
        {
            ParseEnums(realmStatusJson);
            ParsePrimitiveTypes(realmStatusJson);
            ParseConnectedRealms(realmStatusJson);
        }

        private void ParseEnums(JObject realmStatusJson)
        {
            Type = Util.ParseEnum<RealmType>(realmStatusJson, "type");
            Population = Util.ParseEnum<RealmPopulation>(realmStatusJson, "population");
            Locale = Util.ParseEnum<Locale>(realmStatusJson, "locale");
        }

        private void ParsePrimitiveTypes(JObject realmStatusJson)
        {
            Queue = realmStatusJson["queue"].Value<bool>();
            Status = realmStatusJson["status"].Value<bool>();
            Name = realmStatusJson["name"].Value<string>();
            Slug = realmStatusJson["slug"].Value<string>();
            BattleGroup = realmStatusJson["battlegroup"].Value<string>();
            Timezone = realmStatusJson["timezone"].Value<string>();
        }

        private void ParseConnectedRealms(JObject realmStatusJson)
        {
            ConnectedRealms = new List<string>();
            if (realmStatusJson["connected_realms"].HasValues)
            {
                foreach (var connectedRealm in realmStatusJson["connected_realms"].AsJEnumerable())
                {
                    ConnectedRealms.Add(connectedRealm.Value<string>());
                }
            }
        }
    }

}
