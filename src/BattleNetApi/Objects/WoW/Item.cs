using BattleNetApi.Common;
using BattleNetApi.Common.Extensions;
using BattleNetApi.Objects.WoW.DataResources;
using BattleNetApi.Objects.WoW.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace BattleNetApi.Objects.WoW
{
    public class Item
    {
        #region Properties
        public int Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public int Stackable { get; set; }

        public int ItemBind { get; set; }

        // TODO: Include BounsStats property
        public List<BonusStat> BonusStats { get; set; }

        public List<ItemSpell> ItemSpells { get; set; }

        public GoldValue BuyPrice { get; set; }

        public ItemClassDataResource ItemClass { get; set; }

        public ItemSubClassDataResource ItemSubClass { get; set; }

        public int ContainerSlots { get; set; }

        public int InventoryType { get; set; }

        public bool Equippable { get; set; }

        public int ItemLevel { get; set; }

        public int MaxCount { get; set; }

        public int MaxDurability { get; set; }

        public int MinFactionId { get; set; }

        public int MinRepuration { get; set; }

        public ItemQuality Quality { get; set; }

        public GoldValue SellPrice { get; set; }

        public int RequiredSkill { get; set; }

        public int RequiredLevel { get; set; }

        public int RequiredSkillRank { get; set; }

        public ItemSource ItemSource { get; set; }

        public int BaseArmor { get; set; }

        public bool HasSockets { get; set; }

        public bool IsAuctionable { get; set; }

        public int Armor { get; set; }

        public int DisplayInfoId { get; set; }

        public string NameDescription { get; set; }

        public string NameDescriptionColor { get; set; }

        public bool Upgradable { get; set; }

        public bool HeroicTooltip { get; set; }

        public string Context { get; set; }

        public List<int> BonusLists { get; set; }

        public List<string> AvailableContexts { get; set; }

        public BonusSummary BonusSummary { get; set; }

        public ItemTooltipParams TooltipParams { get; set; }

        public int DisenchantingSkillRank { get; set; }

        public int ArtifactId { get; set; }
        #endregion

        internal static Item ParseItemJson(JObject itemJson)
        {
            return new Item(itemJson);
        }

        private Item(JObject itemJson)
        {
            ParseConsistentFields(itemJson);

            ParseOptionalFields(itemJson);
        }

        private void ParseConsistentFields(JObject itemJson)
        {
            AssignConsistentPrimitiveType(i => i.Id, itemJson, "id");
            AssignConsistentPrimitiveType(i => i.Name, itemJson, "name");
            AssignConsistentPrimitiveType(i => i.Icon, itemJson, "icon");
            AssignConsistentPrimitiveType(i => i.ItemLevel, itemJson, "itemLevel");
            AssignConsistentPrimitiveType(i => i.Armor, itemJson, "armor");
            AssignConsistentPrimitiveType(i => i.Context, itemJson, "context");
            AssignConsistentPrimitiveType(i => i.ArtifactId, itemJson, "artifactId");
            Quality = Util.ParseEnum<ItemQuality>(itemJson, "quality");

            ParseConsistentComplexFields(itemJson);
        }

        private void AssignConsistentPrimitiveType<TProperty>(Expression<Func<Item, TProperty>> propertyExpression, JObject itemJson, string jsonKey)
        {
            var memberExpression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            propertyInfo.SetValue(this, itemJson[jsonKey].Value<TProperty>());
        }

        private void ParseConsistentComplexFields(JObject itemJson)
        {
            BuyPrice = GoldValue.BuildGoldValue(itemJson["buyPrice"].Value<int>());
            ParseBonusStats(itemJson);
            ParseItemSpells(itemJson);
        }

        private void ParseBonusStats(JObject itemJson)
        {
            BonusStats = new List<BonusStat>();
            foreach (var bonusStatJson in itemJson["bonusStats"].AsJEnumerable())
            {
                BonusStats.Add(BonusStat.ParseBonusStat(bonusStatJson.Value<JObject>()));
            }
        }

        private void ParseItemSpells(JObject itemJson)
        {
            ItemSpells = new List<ItemSpell>();
            foreach (var itemSpellJson in itemJson["itemSpells"].AsJEnumerable())
            {
                ItemSpells.Add(ItemSpell.ParseItemSpellJson(itemSpellJson.Value<JObject>()));
            }
        }

        private void ParseOptionalFields(JObject itemJson)
        {
            AssignOptionalPrimitiveField(i => i.DisenchantingSkillRank, itemJson, "disenchantingSkillRank");
            AssignOptionalPrimitiveField(i => i.Description, itemJson, "description");
            AssignOptionalPrimitiveField(i => i.Stackable, itemJson, "stackable");
            AssignOptionalPrimitiveField(i => i.ItemBind, itemJson, "itemBind");
            AssignOptionalPrimitiveField(i => i.ContainerSlots, itemJson, "containerSlots");
            AssignOptionalPrimitiveField(i => i.InventoryType, itemJson, "inventoryType");
            AssignOptionalPrimitiveField(i => i.Equippable, itemJson, "equippable");
            AssignOptionalPrimitiveField(i => i.MaxCount, itemJson, "maxCount");
            AssignOptionalPrimitiveField(i => i.MaxDurability, itemJson, "maxDurability");
            AssignOptionalPrimitiveField(i => i.MinFactionId, itemJson, "minFactionId");
            AssignOptionalPrimitiveField(i => i.MinRepuration, itemJson, "minRepuration");
            AssignOptionalPrimitiveField(i => i.RequiredSkill, itemJson, "requiredSkill");
            AssignOptionalPrimitiveField(i => i.RequiredLevel, itemJson, "requiredLevel");
            AssignOptionalPrimitiveField(i => i.RequiredSkillRank, itemJson, "requiredSkillRank");
            AssignOptionalPrimitiveField(i => i.BaseArmor, itemJson, "baseArmor");
            AssignOptionalPrimitiveField(i => i.HasSockets, itemJson, "hasSockets");
            AssignOptionalPrimitiveField(i => i.IsAuctionable, itemJson, "isAuctionable");
            AssignOptionalPrimitiveField(i => i.Armor, itemJson, "armor");
            AssignOptionalPrimitiveField(i => i.DisplayInfoId, itemJson, "displayInfoId");
            AssignOptionalPrimitiveField(i => i.NameDescription, itemJson, "nameDescription");
            AssignOptionalPrimitiveField(i => i.NameDescriptionColor, itemJson, "nameDescriptionColor");
            AssignOptionalPrimitiveField(i => i.Upgradable, itemJson, "upgradable");
            AssignOptionalPrimitiveField(i => i.HeroicTooltip, itemJson, "heroicTooltip");

            ParseOptionalComplextTypes(itemJson);
        }

        private void AssignOptionalPrimitiveField<TProperty>(Expression<Func<Item, TProperty>> propertyExpression, JObject itemJson, string jsonKey)
        {
            if (!itemJson.ContainsKey(jsonKey))
            {
                return;
            }
            var memberExpression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            propertyInfo.SetValue(this, itemJson[jsonKey].Value<TProperty>());
        }

        private void ParseOptionalComplextTypes(JObject itemJson)
        {
            if (itemJson.ContainsKey("tooltipParams"))
            {
                TooltipParams = ItemTooltipParams.ParseItemTooltipParamsJson(itemJson["tooltipParams"].Value<JObject>());
            }
            if (itemJson.ContainsKey("itemClass"))
            {
                ItemClass = ItemClassDataResource.BuildItemClassWithOnlyId(itemJson["itemClass"].Value<int>());
            }
            if (itemJson.ContainsKey("itemSubClass"))
            {
                ItemSubClass = ItemSubClassDataResource.BuildItemSubClassWithOnlyId(itemJson["itemSubClass"].Value<int>());
            }
            if (itemJson.ContainsKey("itemSource"))
            {
                ItemSource = ItemSource.ParseItemSource(itemJson["itemSource"].Value<JObject>());
            }
            if (itemJson.ContainsKey("bonusLists"))
            {
                BonusLists = new List<int>();
                foreach (var bonusListId in itemJson["bonusLists"].AsJEnumerable())
                {
                    BonusLists.Add(bonusListId.Value<int>());
                }
            }
            if (itemJson.ContainsKey("availableContexts"))
            {
                AvailableContexts = new List<string>();
                foreach (var context in itemJson["availableContexts"].AsJEnumerable())
                {
                    AvailableContexts.Add(context.Value<string>());
                }
            }
            if (itemJson.ContainsKey("bonusSummary"))
            {
                BonusSummary = BonusSummary.ParseBonusSummary(itemJson["bonusSummary"].Value<JObject>());
            }
            if (itemJson.ContainsKey("sellPrice"))
            {
                SellPrice = GoldValue.BuildGoldValue(itemJson["sellPrice"].Value<int>());
            }
        }
    }
}
