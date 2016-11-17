using BattleNetApi.Common;
using BattleNetApi.Objects.WoW.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class Achievement
    {
        internal static Achievement BuildFullAchievement(JObject achievementJson)
        {
            return new Achievement(achievementJson);
        }

        internal static Achievement BuildAchievementWithOnlyId(int id)
        {
            return new Achievement(id);
        }

        #region Constructors
        private Achievement(int id)
        {
            Id = id;
        }

        private Achievement(JObject achievementJson)
        {
            ParsePrimitiveTypes(achievementJson);
            Faction = Util.ParseEnum<Faction>(achievementJson, "factionId");
            ParseCriteria(achievementJson["criteria"]);
        }
        #endregion

        #region Public properties
        public int Id { get; private set; }

        public string Title { get; private set; }

        public int Points { get; private set; }

        public string Description { get; private set; }

        //TODO: Include Reward Items array

        public string Icon { get; private set; }

        public List<Criterion> Criteria { get; private set; }

        public bool AccountWide { get; private set; }

        public Faction Faction { get; private set; }

        public DateTime? CompletedTimestamp { get; internal set; }
        #endregion

        #region Private interface
        private void ParsePrimitiveTypes(JObject achievementJson)
        {
            Id = achievementJson["id"].Value<int>();
            Title = achievementJson["title"].Value<string>();
            Points = achievementJson["points"].Value<int>();
            Description = achievementJson["description"].Value<string>();
            Icon = achievementJson["icon"].Value<string>();
            AccountWide = achievementJson["accountWide"].Value<bool>();
            CompletedTimestamp = null;
        }

        private void ParseCriteria(JToken criteriaJson)
        {
            Criteria = new List<Criterion>();
            foreach (JObject criterion in criteriaJson.AsJEnumerable())
            {
                Criteria.Add(new Criterion(criterion));
            }
            Criteria = Criteria.OrderBy(criterion => criterion.OrderIndex).ToList();
        }
        #endregion
    }
}
