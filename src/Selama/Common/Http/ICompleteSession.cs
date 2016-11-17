using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama.Common.Http
{
    public interface ICompleteSession : ISession
    {
        void SetObjectAsJson(string key, object value);
        T GetObjectFromJson<T>(string key);

        void SetString(string key, string value);
        string GetString(string key);
    }
}
