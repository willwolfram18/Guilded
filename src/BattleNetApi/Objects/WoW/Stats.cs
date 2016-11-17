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
        public int Health { get; private set; }

        public PowerType PowerType { get; private set; }

        public int Power { get; private set; }

        #region Main character stats
        public int Strength { get; private set; }

        public int Agility { get; private set; }

        public int Intellect { get; private set; }

        public int Stamina { get; private set; }
        #endregion

        public double SpeedRating { get; private set; }

        public double SpeedRatingBonus { get; private set; }

        public double Crit { get; private set; }

        public int CritRating { get; private set; }

        public double Haste { get; private set; }

        public int HasteRating { get; private set; }

        public double HasteRatingPercent { get; private set; }

        public double Mastery { get; private set; }

        public int MasteryRating { get; private set; }

        public double Leech { get; private set; }

        public double LeechRating { get; private set; }

        public double LeechRatingBonus { get; private set; }

        public int Versatility { get; private set; }

        public double VersatilityDamageDoneBonus { get; private set; }

        public double VersatilityHealingDoneBonus { get; private set; }

        public double VersatilityDamageTakenBonus { get; private set; }

        public double AvoidanceRating { get; private set; }

        public double AvoidanceRatingBonus { get; private set; }

        public double SpellPenetration { get; private set; }

        public double SpellCrit { get; private set; }

        public int SpellCritRating { get; private set; }

        public double Mana5 { get; private set; }

        public double Mana5Combat { get; private set; }

        #region Defensive stats
        public int Armor { get; private set; }

        public double Dodge { get; private set; }

        public int DodgeRating { get; private set; }

        public double Parry { get; private set; }

        public int ParryRating { get; private set; }

        public double Block { get; private set; }

        public int BlockRating { get; private set; }
        #endregion

        #region Weapon damage stats
        #region Main hand
        public double MainHandDamageMin { get; private set; }

        public double MainHandDamageMax { get; private set; }

        public double MainHandSpeed { get; private set; }

        public double MainHandDps { get; private set; }
        #endregion

        #region Offhand
        public double OffHandDamageMin { get; private set; }

        public double OffHandDamageMax { get; private set; }

        public double OffHandSpeed { get; private set; }

        public double OffHandDps { get; private set; }
        #endregion

        #region Ranged
        public double RangedDamageMin { get; private set; }

        public double RangedDamageMax { get; private set; }

        public double RangedSpeed { get; private set; }

        public double RangedDps { get; private set; }
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
