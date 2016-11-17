using BattleNetApi.Common;
using BattleNetApi.Objects.WoW.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class GuildNews
    {
        #region Properties
        public GuildNewsType Type { get; private set; }

        public string CharacterName { get; private set; }

        public long BattleNetTimestamp { get; private set; }

        public DateTime DateTimeTimestamp { get; private set; }

        public string Context { get; private set; }

        public List<int> BonusLists { get; private set; }

        #endregion

        internal static GuildNews ParseGuildNews(JObject guildNewsJson, string timezone)
        {
            string context = guildNewsJson["type"].Value<string>();
            switch (context)
            {
                case "itemLoot":
                case "itemPurchase":
                case "itemCraft":
                    return GuildNewsPlayerItem.BuildPlayerItemNews(guildNewsJson, timezone);
                case "playerAchievement":
                case "guildAchievement":
                    return GuildNewsAchievement.BuildPlayerAchievement(guildNewsJson, timezone);
                default:
                    return new GuildNews(guildNewsJson, timezone);
            }
        }

        protected GuildNews(JObject guildNewsJson, string timezone)
        {
            Type = Util.ParseEnum<GuildNewsType>(guildNewsJson, "type");
            Context = guildNewsJson["context"].Value<string>();
            BattleNetTimestamp = guildNewsJson["timestamp"].Value<long>();
            DateTimeTimestamp = Util.BuildUnixTimestamp(BattleNetTimestamp, timezone);
            // TODO: Watch for character not being present in json
            CharacterName = guildNewsJson["character"].Value<string>();

            ParseBonusLists(guildNewsJson["bonusLists"]);
        }

        private void ParseBonusLists(JToken bonusListJson)
        {
            BonusLists = new List<int>();
            foreach (var bonus in bonusListJson.AsEnumerable())
            {
                BonusLists.Add(bonus.Value<int>());
            }
        }
    }
}
