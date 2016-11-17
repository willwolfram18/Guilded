using BattleNetApi.Common.ExtensionMethods;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BattleNetApi.Objects.WoW
{
    public class CharacterEquipment
    {
        #region Properties
        public int AverageItemLevel { get; private set; }

        public int AverageItemLevelEquipped { get; private set; }

        public Item Head { get; private set; }

        public Item Neck { get; private set; }

        public Item Shoulder { get; private set; }

        public Item Back { get; private set; }

        public Item Chest { get; private set; }

        public Item Shirt { get; private set; }

        public Item Wrist { get; private set; }

        public Item Hands { get; private set; }

        public Item Waist { get; private set; }

        public Item Legs { get; private set; }

        public Item Feet { get; private set; }

        public Item Finger1 { get; private set; }

        public Item Finger2 { get; private set; }

        public Item Trinket1 { get; private set; }

        public Item Trinket2 { get; private set; }
        #endregion

        internal static CharacterEquipment BuildCharacterEquipment(JObject equipmentJson)
        {
            return new CharacterEquipment(equipmentJson);
        }

        private CharacterEquipment(JObject equipmentJson)
        {
            AverageItemLevel = equipmentJson["averageItemLevel"].Value<int>();
            AverageItemLevelEquipped = equipmentJson["averageItemLevelEquipped"].Value<int>();

            AssignEquipmentSlot(e => e.Back, equipmentJson, "back");
            AssignEquipmentSlot(e => e.Chest, equipmentJson, "chest");
            AssignEquipmentSlot(e => e.Feet, equipmentJson, "feet");
            AssignEquipmentSlot(e => e.Finger1, equipmentJson, "finger1");
            AssignEquipmentSlot(e => e.Finger2, equipmentJson, "finger2");
            AssignEquipmentSlot(e => e.Hands, equipmentJson, "hands");
            AssignEquipmentSlot(e => e.Head, equipmentJson, "head");
            AssignEquipmentSlot(e => e.Legs, equipmentJson, "legs");
            AssignEquipmentSlot(e => e.Neck, equipmentJson, "neck");
            AssignEquipmentSlot(e => e.Shirt, equipmentJson, "shirt");
            AssignEquipmentSlot(e => e.Shoulder, equipmentJson, "shoulder");
            AssignEquipmentSlot(e => e.Trinket1, equipmentJson, "trinket1");
            AssignEquipmentSlot(e => e.Trinket2, equipmentJson, "trinket2");
            AssignEquipmentSlot(e => e.Waist, equipmentJson, "waist");
            AssignEquipmentSlot(e => e.Wrist, equipmentJson, "wrist");
        }

        private void AssignEquipmentSlot(Expression<Func<CharacterEquipment, Item>> propertyExpression, JObject equipmentJson, string jsonKey)
        {
            if (!equipmentJson.ContainsKey(jsonKey))
            {
                return;
            }
            var memberExpression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            propertyInfo.SetValue(this, Item.ParseItemJson(equipmentJson[jsonKey].Value<JObject>()));
        }
    }
}
