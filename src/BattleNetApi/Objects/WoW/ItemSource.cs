using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class ItemSource
    {
        #region Properties
        public int SourceId { get; set; }

        public string SourceType { get; set; }
        #endregion

        internal static ItemSource ParseItemSource(JObject itemSourceJson)
        {
            return new ItemSource
            {
                SourceId = itemSourceJson["sourceId"].Value<int>(),
                SourceType = itemSourceJson["sourceType"].Value<string>()
            };
        }
    }
}
