using BattleNetApi.Common.ExtensionMethods;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BattleNetApi.Objects.WoW.DataResources
{
    public class ItemClassDataResource
    {
        #region Properties
        public int Id { get; private set; }

        public string Name { get; private set; }

        public List<ItemSubClassDataResource> SubClasses { get; private set; }
        #endregion

        internal static List<ItemClassDataResource> BuildItemClassListFromJson(JObject itemClassesJson)
        {
            List<ItemClassDataResource> itemClasses = new List<ItemClassDataResource>();
            foreach (var itemClass in itemClassesJson["classes"].AsJEnumerable())
            {
                itemClasses.Add(BuildItemClassFromJson(itemClass.Value<JObject>()));
            }
            return itemClasses;
        }

        internal static ItemClassDataResource BuildItemClassFromJson(JObject itemClassJson)
        {
            return new ItemClassDataResource(itemClassJson);
        }

        internal static ItemClassDataResource BuildItemClassWithOnlyId(int id)
        {
            return new ItemClassDataResource(id);
        }

        private ItemClassDataResource(int id)
        {
            Id = id;
        }

        private ItemClassDataResource(JObject itemClassJson)
        {
            Id = itemClassJson["class"].Value<int>();
            Name = itemClassJson["name"].Value<string>();

            SubClasses = new List<ItemSubClassDataResource>();
            if (itemClassJson.ContainsKey("subclasses"))
            {
                foreach (var subclass in itemClassJson["subclasses"].AsJEnumerable())
                {
                    SubClasses.Add(ItemSubClassDataResource.BuildItemSubClass(subclass.Value<JObject>()));
                }
            }
        }
    }
}
