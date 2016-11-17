using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class GuildNewsAchievement : GuildNews
    {
        public Achievement Achievement { get; private set; }

        internal static GuildNewsAchievement BuildPlayerAchievement(JObject playerAchievementNewsJson, string timezone)
        {
            return new GuildNewsAchievement(playerAchievementNewsJson, timezone);
        }

        private GuildNewsAchievement(JObject playerAchievementNewsJson, string timezone) : base(playerAchievementNewsJson, timezone)
        {
            Achievement = WoW.Achievement.BuildFullAchievement(playerAchievementNewsJson["achievement"].Value<JObject>());
        }
    }
}
