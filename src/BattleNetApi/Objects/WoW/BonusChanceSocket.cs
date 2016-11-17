using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class BonusChanceSocket
    {
        public string SocketType { get; private set; }

        internal static BonusChanceSocket ParseBonusChanceSocketJson(JObject bonusChanceSocketJson)
        {
            return new BonusChanceSocket
            {
                SocketType = bonusChanceSocketJson["socketType"].Value<string>(),
            };
        }
    }
}
