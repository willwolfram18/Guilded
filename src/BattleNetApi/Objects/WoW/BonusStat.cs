using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class BonusStat
    {
        #region Properties
        public int Stat { get; set; }

        public int Amount { get; set; }
        #endregion

        internal static BonusStat ParseBonusStat(JObject bonusStatJson)
        {
            return new BonusStat
            {
                Stat = bonusStatJson["stat"].Value<int>(),
                Amount = bonusStatJson["amount"].Value<int>(),
            };
        }
    }
}
