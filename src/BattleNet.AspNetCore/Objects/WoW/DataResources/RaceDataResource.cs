using BattleNetApi.Common;
using BattleNetApi.Objects.WoW.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace BattleNetApi.Objects.WoW.DataResources
{
    public class RaceDataResource
    {
        internal static List<RaceDataResource> BuildRacesList(JObject racesJson)
        {
            List<RaceDataResource> races = new List<RaceDataResource>();
            foreach (var raceDataResource in racesJson["races"].AsJEnumerable())
            {
                races.Add(new RaceDataResource(raceDataResource.Value<JObject>()));
            }
            return races;
        }

        private RaceDataResource(JObject raceDataResourceJson)
        {
            Id = raceDataResourceJson["id"].Value<int>();
            Mask = raceDataResourceJson["mask"].Value<int>();
            Race = (Race)Enum.Parse(typeof(Race), Id.ToString());
            Faction = Util.ParseEnum<Faction>(raceDataResourceJson, "side");
        }

        public int Id { get; private set; }

        public int Mask { get; private set; }

        public Race Race { get; private set; }
        
        public Faction Faction { get; private set; }

    }
}
