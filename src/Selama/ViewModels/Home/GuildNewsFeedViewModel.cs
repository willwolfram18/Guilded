using BattleNetApi.Objects.WoW;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama.ViewModels.Home
{
    public class GuildNewsFeedViewModel : IComparable<GuildNewsFeedViewModel>
    {
        #region Properties
        #region Public properties
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime Timestamp { get; private set; }

        public HtmlString Content { get; private set; }
        #endregion
        #endregion

        #region Constructors
        public GuildNewsFeedViewModel(DateTime timestamp, string content)
        {
            Timestamp = timestamp;
            Content = new HtmlString(content);
        }
        #endregion

        #region Methods
        #region Public methods
        public static GuildNewsFeedViewModel CreateFromBattleNetNews(GuildNews newsItem)
        {
            switch (newsItem.Type)
            {
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemLoot:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemPurchase:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemCraft:
                    return BuildGuildItemNews(newsItem as GuildNewsPlayerItem);
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.PlayerAchievement:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.GuildAchievement:
                    return BuildGuildAchievementNews(newsItem as GuildNewsAchievement);
                default:
                    throw new NotImplementedException();
            }
        }

        public int CompareTo(GuildNewsFeedViewModel other)
        {
            return -(Timestamp.CompareTo(other.Timestamp));
        }
        #endregion

        #region Private methods
        private static GuildNewsFeedViewModel BuildGuildItemNews(GuildNewsPlayerItem itemNews)
        {
            return new GuildNewsFeedViewModel(
                itemNews.DateTimeTimestamp,
                string.Format(
                    "{0} obtained {1}.", 
                    itemNews.CharacterName, 
                    ItemTag(itemNews)
                )
            );
        }
        private static string ItemTag(GuildNewsPlayerItem itemNews)
        {
            string baseTag = "<a href = '//wowhead.com/item={0}' class='item' target='_blank' rel='{1}' >Item {0}</a>";
            string relAttribute = "";
            if (itemNews.BonusLists != null && itemNews.BonusLists.Count > 0)
            {
                relAttribute = "bonus=" + string.Join(":", itemNews.BonusLists);
            }
            return string.Format(baseTag, itemNews.ItemId, relAttribute);
        }

        private static GuildNewsFeedViewModel BuildGuildAchievementNews(GuildNewsAchievement achievementNews)
        {
            string achievementEarner = DetermineAchievementEarner(achievementNews);
            return new GuildNewsFeedViewModel(
                achievementNews.DateTimeTimestamp,
                string.Format(
                    "{0} earned {1} for {2} points.", 
                    achievementEarner, 
                    AchievementTag(achievementNews), 
                    achievementNews.Achievement.Points
                )
            );
        }
        private static string AchievementTag(GuildNewsAchievement achievementNews)
        {
            string baseTag = "<a class='achievement' href='//wowhead.com/achievement={0}' rel='achievement={0}&who={1}&when={2}' target='_blank'>Achievement {0}</a>";
            return string.Format(baseTag, achievementNews.Achievement.Id,
                achievementNews.CharacterName, achievementNews.BattleNetTimestamp);
        }

        private static string DetermineAchievementEarner(GuildNewsAchievement achievementNews)
        {
            switch (achievementNews.Type)
            {
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.PlayerAchievement:
                    return achievementNews.CharacterName;
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.GuildAchievement:
                    return "Selama Ashalanore";
                default:
                    throw new NotImplementedException(string.Format("No case implemented for guild news type {0}", achievementNews.Type.ToString()));
            }
        }
        #endregion
        #endregion
    }
}
