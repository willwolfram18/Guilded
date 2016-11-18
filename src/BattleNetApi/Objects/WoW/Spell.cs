using BattleNetApi.Common.Extensions;
using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class Spell
    {
        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public string Description { get; set; }

        public string Range { get; set; }

        public string CastTime { get; set; }

        public string CoolDown { get; set; }

        public string PowerCost { get; set; }
        #endregion

        internal static Spell ParseSpellJson(JObject spellJson)
        {
            return new Spell(spellJson);
        }

        private Spell(JObject spellJson)
        {
            Id = spellJson["id"].Value<int>();
            Name = spellJson["name"].Value<string>();
            Icon = spellJson["icon"].Value<string>();
            Description = spellJson["description"].Value<string>();
            CastTime = spellJson["castTime"].Value<string>();

            ParseOptionalProperties(spellJson);
        }

        private void ParseOptionalProperties(JObject spellJson)
        {
            if (spellJson.ContainsKey("range"))
            {
                Range = spellJson["range"].Value<string>();
            }
            if (spellJson.ContainsKey("powerCost"))
            {
                PowerCost = spellJson["powerCost"].Value<string>();
            }
            if (spellJson.ContainsKey("cooldown"))
            {
                CoolDown = spellJson["cooldown"].Value<string>();
            }
        }
    }
}
