using BattleNetApi.Common;

namespace BattleNetApi.Objects.WoW
{
    public class GoldValue
    {
        #region Properties
        public double BuyPriceInCopper { get; private set; }

        public double BuyPriceInSilver
        {
            get
            {
                return BuyPriceInCopper / Util.COPPER_IN_SILVER;
            }
        }

        public double BuyPriceInGold
        {
            get
            {
                return BuyPriceInSilver / Util.SILVER_IN_GOLD;
            }
        }
        #endregion

        internal static GoldValue BuildGoldValue(int priceInCopper)
        {
            return new GoldValue { BuyPriceInCopper = priceInCopper };
        }
    }
}
