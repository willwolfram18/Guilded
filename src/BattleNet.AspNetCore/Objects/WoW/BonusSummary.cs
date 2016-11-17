using BattleNetApi.Common.ExtensionMethods;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BattleNetApi.Objects.WoW
{
    public class BonusSummary
    {
        #region Properties
        public List<int> DefaultBonusLists { get; private set; }

        public List<int> ChanceBonusLists { get; private set; }

        public List<BonusChance> BonusChances { get; private set; }
        #endregion

        internal static BonusSummary ParseBonusSummary(JObject bonusSummaryJson)
        {
            return new BonusSummary(bonusSummaryJson);
        }

        private BonusSummary(JObject bonusSummaryJson)
        {
            ParseDefaultBonuses(bonusSummaryJson);
            ParseChanceBonusLists(bonusSummaryJson);
            ParseBonusChances(bonusSummaryJson);
        }

        private void ParseDefaultBonuses(JObject bonusSummaryJson)
        {
            DefaultBonusLists = new List<int>();
            if (bonusSummaryJson.ContainsKey("defaultBonusLists") &&
                bonusSummaryJson["defaultBonusLists"].HasValues)
            {
                foreach (var bonusDefaultId in bonusSummaryJson["defaultBonusLists"].AsJEnumerable())
                {
                    DefaultBonusLists.Add(bonusDefaultId.Value<int>());
                }
            }
        }

        private void ParseChanceBonusLists(JObject bonusSummaryJson)
        {
            ChanceBonusLists = new List<int>();
            if (bonusSummaryJson.ContainsKey("chanceBonusLists") &&
                bonusSummaryJson["chanceBonusLists"].HasValues)
            {
                foreach (var bonusDefaultId in bonusSummaryJson["chanceBonusLists"].AsJEnumerable())
                {
                    ChanceBonusLists.Add(bonusDefaultId.Value<int>());
                }
            }
        }

        private void ParseBonusChances(JObject bonusSummaryJson)
        {
            BonusChances = new List<BonusChance>();
            if (bonusSummaryJson.ContainsKey("bonusChances") && bonusSummaryJson["bonusChances"].HasValues)
            {
                foreach (var bonusChanceJson in bonusSummaryJson["bonusChances"].AsJEnumerable())
                {
                    BonusChances.Add(BonusChance.ParseBonusChanceJson(bonusChanceJson.Value<JObject>()));
                }
            }
        }
    }
}
