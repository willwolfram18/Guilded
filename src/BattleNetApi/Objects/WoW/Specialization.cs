using BattleNetApi.Common;
using BattleNetApi.Common.Extensions;
using BattleNetApi.Objects.WoW.Enums;
using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class Specialization
    {
        public static Specialization BuildCharacterSpecialization(JObject specializationJson, Class classForSpec)
        {
            return new Specialization(specializationJson, classForSpec) { Selected = true };
        }

        private Specialization(JObject specializationJson, Class classForSpec)
        {
            ClassSpecBelongsTo = classForSpec;
            Name = specializationJson["name"].Value<string>();
            Selected = specializationJson.ContainsKey("selected");
            Role = Util.ParseEnum<Role>(specializationJson, "role");
            SortOrder = specializationJson["order"].Value<int>();
            Description = specializationJson["description"].Value<string>();
        }

        public string Name { get; set; }

        public Class ClassSpecBelongsTo { get; set; }

        public Role Role { get; set; }

        public bool Selected { get; set; }

        public int SortOrder { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return string.Format("{{ Name: \"{0}\", Description: \"{1}\", Role: Role.{2}, Selected: {3}, SortOrder: {4} }}",
                Name, Description, Role, Selected, SortOrder);
        }
    }
}
