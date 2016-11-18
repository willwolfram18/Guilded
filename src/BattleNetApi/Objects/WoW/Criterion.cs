using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class Criterion
    {
        internal Criterion(JObject criterionJson)
        {
            Id = criterionJson["id"].Value<int>();
            Description = criterionJson["description"].Value<string>();
            OrderIndex = criterionJson["orderIndex"].Value<int>();
            Max = criterionJson["max"].Value<int>();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public int OrderIndex { get; set; }

        public int Max { get; set; }
    }
}
