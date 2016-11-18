using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class ItemUpgradeInfo
    {
        #region Properties
        public int Current { get; set; }

        public int Total { get; set; }

        public int ItemLevelIncrement { get; set; }
        #endregion

        public ItemUpgradeInfo(JObject itemUpgradeInfoJson)
        {
            Current = itemUpgradeInfoJson["current"].Value<int>();
            Total = itemUpgradeInfoJson["total"].Value<int>();
            ItemLevelIncrement = itemUpgradeInfoJson["itemLevelIncrement"].Value<int>();
        }
    }
}
