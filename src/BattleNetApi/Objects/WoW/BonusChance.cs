using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using BattleNetApi.Common.ExtensionMethods;

namespace BattleNetApi.Objects.WoW
{
    public class BonusChance
    {
        #region Properties
        public string ChanceType { get; private set; }

        public List<BonusChanceStat> Stats { get; private set; }

        public List<BonusChanceSocket> Sockets { get; private set; }
        #endregion

        internal static BonusChance ParseBonusChanceJson(JObject bonusChanceJson)
        {
            return new BonusChance(bonusChanceJson);
        }

        private BonusChance(JObject bonusChanceJson)
        {
            ChanceType = bonusChanceJson["chanceType"].Value<string>();
            ParseStats(bonusChanceJson);
            ParseSockets(bonusChanceJson);
        }

        private void ParseStats(JObject bonusChanceJson)
        {
            Stats = new List<BonusChanceStat>();
            if (bonusChanceJson.ContainsKey("stats") && bonusChanceJson["stats"].HasValues)
            {
                foreach (var bonusStatJson in bonusChanceJson["stats"].AsJEnumerable())
                {
                    Stats.Add(BonusChanceStat.ParseBonusChanceStatJson(bonusStatJson.Value<JObject>()));
                }
            }
        }

        private void ParseSockets(JObject bonusChanceJson)
        {
            Sockets = new List<BonusChanceSocket>();
            if (bonusChanceJson.ContainsKey("sockets") && bonusChanceJson["sockets"].HasValues)
            {
                foreach (var bonusSocketJson in bonusChanceJson["sockets"].AsJEnumerable())
                {
                    Sockets.Add(BonusChanceSocket.ParseBonusChanceSocketJson(bonusSocketJson.Value<JObject>()));
                }
            }
        }
    }
}
