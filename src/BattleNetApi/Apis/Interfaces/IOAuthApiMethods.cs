using BattleNetApi.Objects.WoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Apis.Interfaces
{
    public interface IOAuthApiMethods
    {
        Task<IEnumerable<Character>> WowProfileAsync(string accessToken);
    }
}
