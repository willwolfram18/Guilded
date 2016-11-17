using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class GuildNewsPlayerItem : GuildNews
    {
        public int ItemId { get; private set; }

        internal static GuildNewsPlayerItem BuildPlayerItemNews(JObject playerItemNewsJson, string timezone)
        {
            return new GuildNewsPlayerItem(playerItemNewsJson, timezone);
        }

        private GuildNewsPlayerItem(JObject playerItemNewsJson, string timezone) : base(playerItemNewsJson, timezone)
        {
            ItemId = playerItemNewsJson["itemId"].Value<int>();
        }
    }
}
