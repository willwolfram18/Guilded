using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW.DataResources
{
    public class ItemSubClassDataResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        internal static ItemSubClassDataResource BuildItemSubClass(JObject itemSubClassJson)
        {
            return new ItemSubClassDataResource(itemSubClassJson);
        }

        internal static ItemSubClassDataResource BuildItemSubClassWithOnlyId(int id)
        {
            return new ItemSubClassDataResource(id);
        }

        private ItemSubClassDataResource(int id)
        {
            Id = id;
        }

        private ItemSubClassDataResource(JObject itemSubClassJson)
        {
            Id = itemSubClassJson["subclass"].Value<int>();
            Name = itemSubClassJson["name"].Value<string>();
        }
    }
}
