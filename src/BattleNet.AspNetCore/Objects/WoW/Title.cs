using BattleNetApi.Common.ExtensionMethods;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class Title
    {
        internal static List<Title> BuildListOfTitles(JObject jsonCharacter)
        {
            List<Title> titles = new List<Title>();
            if (jsonCharacter.ContainsKey("titles"))
            {
                foreach (var titleJson in jsonCharacter["titles"].AsEnumerable())
                {
                    titles.Add(new Title(titleJson));
                }
            }
            return titles;
        }

        internal Title(JToken titleJson)
        {
            Id = titleJson["id"].Value<int>();
            Name = titleJson["name"].Value<string>().Replace("%s", "{0}");
            Selected = titleJson.ContainsKey("selected");
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public bool Selected { get; private set; }
    }
}
