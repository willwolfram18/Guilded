using BattleNetApi.Common;
using BattleNetApi.Objects.WoW.Enums;
using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class Stats
    {
        internal Stats(JObject statsJson)
        {
            Health = statsJson["health"].Value<int>();
            PowerType = Util.ParseEnum<PowerType>(statsJson, "powerType");
            Power = statsJson["power"].Value<int>();

            Strength = statsJson["str"].Value<int>();
            Agility = statsJson["agi"].Value<int>();
            Intellect = statsJson["int"].Value<int>();
            Stamina = statsJson["sta"].Value<int>();

            SpeedRating = statsJson["speedRating"].Value<double>();
            SpeedRatingBonus = statsJson["speedRatingBonus"].Value<double>();

            Crit = statsJson["crit"].Value<double>();
            CritRating = statsJson["critRating"].Value<int>();

            Haste = statsJson["haste"].Value<double>();
            HasteRating = statsJson["hasteRating"].Value<int>();
            HasteRatingPercent = statsJson["hasteRatingPercent"].Value<double>();

            Mastery = statsJson["mastery"].Value<double>();
            MasteryRating = statsJson["masteryRating"].Value<int>();

            Leech = statsJson["leech"].Value<double>();
            LeechRating = statsJson["leechRating"].Value<double>();
            LeechRatingBonus = statsJson["leechRatingBonus"].Value<double>();

            Versatility = statsJson["versatility"].Value<int>();
            VersatilityDamageDoneBonus = statsJson["versatilityDamageDoneBonus"].Value<double>();
            VersatilityDamageTakenBonus = statsJson["versatilityDamageTakenBonus"].Value<double>();
            VersatilityHealingDoneBonus = statsJson["versatilityHealingDoneBonus"].Value<double>();

            AvoidanceRating = statsJson["avoidanceRating"].Value<double>();
            AvoidanceRatingBonus = statsJson["avoidanceRatingBonus"].Value<double>();

            SpellPenetration = statsJson["spellPen"].Value<double>();
            SpellCrit = statsJson["spellCrit"].Value<double>();
            SpellCritRating = statsJson["spellCritRating"].Value<int>();

            Mana5 = statsJson["mana5"].Value<double>();
            Mana5Combat = statsJson["mana5Combat"].Value<double>();

            InitDefensiveStats(statsJson);

            InitMainHandStats(statsJson);
            InitOffHandStats(statsJson);
            InitRangedStats(statsJson);
        }

        #region Properties
        public int Health { get; set; }

        public PowerType PowerType { get; set; }

        public int Power { get; set; }

        #region Main character stats
        public int Strength { get; set; }

        public int Agility { get; set; }

        public int Intellect { get; set; }

        public int Stamina { get; set; }
        #endregion

        public double SpeedRating { get; set; }

        public double SpeedRatingBonus { get; set; }

        public double Crit { get; set; }

        public int CritRating { get; set; }

        public double Haste { get; set; }

        public int HasteRating { get; set; }

        public double HasteRatingPercent { get; set; }

        public double Mastery { get; set; }

        public int MasteryRating { get; set; }

        public double Leech { get; set; }

        public double LeechRating { get; set; }

        public double LeechRatingBonus { get; set; }

        public int Versatility { get; set; }

        public double VersatilityDamageDoneBonus { get; set; }

        public double VersatilityHealingDoneBonus { get; set; }

        public double VersatilityDamageTakenBonus { get; set; }

        public double AvoidanceRating { get; set; }

        public double AvoidanceRatingBonus { get; set; }

        public double SpellPenetration { get; set; }

        public double SpellCrit { get; set; }

        public int SpellCritRating { get; set; }

        public double Mana5 { get; set; }

        public double Mana5Combat { get; set; }

        #region Defensive stats
        public int Armor { get; set; }

        public double Dodge { get; set; }

        public int DodgeRating { get; set; }

        public double Parry { get; set; }

        public int ParryRating { get; set; }

        public double Block { get; set; }

        public int BlockRating { get; set; }
        #endregion

        #region Weapon damage stats
        #region Main hand
        public double MainHandDamageMin { get; set; }

        public double MainHandDamageMax { get; set; }

        public double MainHandSpeed { get; set; }

        public double MainHandDps { get; set; }
        #endregion

        #region Offhand
        public double OffHandDamageMin { get; set; }

        public double OffHandDamageMax { get; set; }

        public double OffHandSpeed { get; set; }

        public double OffHandDps { get; set; }
        #endregion

        #region Ranged
        public double RangedDamageMin { get; set; }

        public double RangedDamageMax { get; set; }

        public double RangedSpeed { get; set; }

        public double RangedDps { get; set; }
        #endregion
        #endregion
        #endregion

        #region Private interface
        private void InitDefensiveStats(JObject statsJson)
        {
            Armor = statsJson["armor"].Value<int>();
            Dodge = statsJson["dodge"].Value<double>();
            DodgeRating = statsJson["dodgeRating"].Value<int>();
            Parry = statsJson["parry"].Value<double>();
            ParryRating = statsJson["parryRating"].Value<int>();
            Block = statsJson["block"].Value<double>();
            BlockRating = statsJson["blockRating"].Value<int>();
        }

        private void InitMainHandStats(JObject statsJson)
        {
            MainHandDamageMin = statsJson["mainHandDmgMin"].Value<double>();
            MainHandDamageMax = statsJson["mainHandDmgMax"].Value<double>();
            MainHandSpeed = statsJson["mainHandSpeed"].Value<double>();
            MainHandDps = statsJson["mainHandDps"].Value<double>();
        }

        private void InitOffHandStats(JObject statsJson)
        {
            OffHandDamageMin = statsJson["offHandDmgMin"].Value<double>();
            OffHandDamageMax = statsJson["offHandDmgMax"].Value<double>();
            OffHandSpeed = statsJson["offHandSpeed"].Value<double>();
            OffHandDps = statsJson["offHandDps"].Value<double>();
        }

        private void InitRangedStats(JObject statsJson)
        {
            RangedDamageMin = statsJson["rangedDmgMin"].Value<double>();
            RangedDamageMax = statsJson["rangedDmgMax"].Value<double>();
            RangedSpeed = statsJson["rangedSpeed"].Value<double>();
            RangedDps = statsJson["rangedDps"].Value<double>();
        }
        #endregion
    }
}
