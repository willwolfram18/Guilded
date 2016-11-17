using BattleNetApi.Common.ExtensionMethods;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class ItemTooltipParams
    {
        #region Properties
        public List<int> GemIds { get; private set; }

        public int? TransmogItemId { get; private set; }

        public int? EnchantmentId { get; private set; }

        public ItemUpgradeInfo Upgrade { get; private set; }

        public List<int> ItemSetIds { get; private set; }
        #endregion

        internal static ItemTooltipParams ParseItemTooltipParamsJson(JObject itemTooltipJson)
        {
            return new ItemTooltipParams(itemTooltipJson);
        }

        private ItemTooltipParams(JObject itemTooltipJson)
        {
            if (itemTooltipJson.ContainsKey("transmogItem"))
            {
                TransmogItemId = itemTooltipJson["transmogItem"].Value<int>();
            }
            if (itemTooltipJson.ContainsKey("enchant"))
            {
                EnchantmentId = itemTooltipJson["enchant"].Value<int>();
            }

            GetItemGemIds(itemTooltipJson);
            GetItemSetIds(itemTooltipJson);
            if (itemTooltipJson.ContainsKey("upgrade"))
            {
                Upgrade = new ItemUpgradeInfo(itemTooltipJson["upgrade"].Value<JObject>());
            }
        }

        private void GetItemGemIds(JObject itemTooltipJson)
        {
            // remove the "gem" portion of the key "gem#" to allow ordering by the gem position number
            var gemKeys = itemTooltipJson.Keys().Where(k => k.StartsWith("gem"))
                .OrderBy(s => Int32.Parse(s.Replace("gem", "")));

            GemIds = new List<int>();
            foreach (var gemId in gemKeys)
            {
                GemIds.Add(itemTooltipJson[gemId].Value<int>());
            }
        }

        private void GetItemSetIds(JObject itemTooltipJson)
        {
            ItemSetIds = new List<int>();
            if (itemTooltipJson.ContainsKey("set"))
            {
                foreach (var itemSetIdToken in itemTooltipJson["set"].AsJEnumerable())
                {
                    ItemSetIds.Add(itemSetIdToken.Value<int>());
                }
            }
        }
    }
}
