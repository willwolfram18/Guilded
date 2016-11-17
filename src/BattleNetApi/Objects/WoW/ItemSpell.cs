using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class ItemSpell
    {
        #region Properties
        public int SpellId { get; private set; }

        public Spell Spell { get; private set; }

        public int nCharges { get; private set; }

        public bool Consumable { get; private set; }

        public int CategoryId { get; private set; }

        public string Trigger { get; private set; }
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
