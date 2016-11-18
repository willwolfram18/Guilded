using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class ItemSpell
    {
        #region Properties
        public int SpellId { get; set; }

        public Spell Spell { get; set; }

        public int nCharges { get; set; }

        public bool Consumable { get; set; }

        public int CategoryId { get; set; }

        public string Trigger { get; set; }
        #endregion

        internal static ItemSpell ParseItemSpellJson(JObject itemSpellJson)
        {
            return new ItemSpell(itemSpellJson);
        }

        private ItemSpell(JObject itemSpellJson)
        {
            SpellId = itemSpellJson["spellId"].Value<int>();
            Spell = Spell.ParseSpellJson(itemSpellJson["spell"].Value<JObject>());
            nCharges = itemSpellJson["nCharges"].Value<int>();
            Consumable = itemSpellJson["consumable"].Value<bool>();
            CategoryId = itemSpellJson["categoryId"].Value<int>();
            Trigger = itemSpellJson["trigger"].Value<string>();
        }
    }
}
