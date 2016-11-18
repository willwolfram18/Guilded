using BattleNetApi.Objects.WoW;
using BattleNetApi.Objects.WoW.DataResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Apis.Interfaces
{
    public interface IWowCommunityApiMethods
    {
        Task<Achievement> GetAchievementAsync(int id);
        Task<Character> GetCharacterProfileAsync(string realm, string characterName, params string[] fields);
        Task<Guild> GetGuildProfileAsync(string realm, string guildName, params string[] fields);
        Task<Item> GetItemAsync(int id);
        Task<List<RaceDataResource>> GetCharacterRacesAsync();
        Task<List<ItemClassDataResource>> GetItemClassesAsync();
        Task<List<RealmStatus>> GetRealmStatusesAsync(params string[] realms);
    }
}
