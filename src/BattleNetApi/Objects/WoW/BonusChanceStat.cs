using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class BonusChanceStat
    {
        #region Properties
        public string Stat { get; set; }

        public int Delta { get; set; }
        #endregion

        internal static BonusChanceStat ParseBonusChanceStatJson(JObject bonusChanceStatJson)
        {
            return new BonusChanceStat
            {
                Stat = bonusChanceStatJson["statId"].Value<string>(),
                Delta = bonusChanceStatJson["delta"].Value<int>(),
            };
        }
    }
}
