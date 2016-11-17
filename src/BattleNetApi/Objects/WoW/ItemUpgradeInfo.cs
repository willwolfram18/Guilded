using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class ItemUpgradeInfo
    {
        #region Properties
        public int Current { get; private set; }

        public int Total { get; private set; }

        public int ItemLevelIncrement { get; private set; }
        #endregion

        public ItemUpgradeInfo(JObject itemUpgradeInfoJson)
        {
            Current = itemUpgradeInfoJson["current"].Value<int>();
            Total = itemUpgradeInfoJson["total"].Value<int>();
            ItemLevelIncrement = itemUpgradeInfoJson["itemLevelIncrement"].Value<int>();
        }
    }
}
