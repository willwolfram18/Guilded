using BattleNetApi.Common.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BattleNetApi.Objects.WoW
{
    public class CharacterEquipment
    {
        #region Properties
        public int AverageItemLevel { get; set; }

        public int AverageItemLevelEquipped { get; set; }

        public Item Head { get; set; }

        public Item Neck { get; set; }

        public Item Shoulder { get; set; }

        public Item Back { get; set; }

        public Item Chest { get; set; }

        public Item Shirt { get; set; }

        public Item Wrist { get; set; }

        public Item Hands { get; set; }

        public Item Waist { get; set; }

        public Item Legs { get; set; }

        public Item Feet { get; set; }

        public Item Finger1 { get; set; }

        public Item Finger2 { get; set; }

        public Item Trinket1 { get; set; }

        public Item Trinket2 { get; set; }
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
