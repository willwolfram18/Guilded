using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Selama.Common.Http
{
    /// <summary>
    /// Acts as a replacement for the <see cref="HttpContext.Session"/> object
    /// within controllers
    /// </summary>
    public class CompleteSession : ICompleteSession
    {
        private ISession Session { get; set; }

        public CompleteSession(ISession session)
        {
            Session = session;
        }

        public string Id
        {
            get
            {
                return Session.Id;
            }
        }

        public bool IsAvailable
        {
            get
            {
                return Session.IsAvailable;
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return Session.Keys;
            }
        }

        public void Clear()
        {
            Session.Clear();
        }

        public async Task CommitAsync()
        {
            await Session.CommitAsync();
        }

        public T GetObjectFromJson<T>(string key)
        {
            return JsonConvert.DeserializeObject<T>(GetString(key));
        }

        public string GetString(string key)
        {
            return Session.GetString(key);
        }

        public async Task LoadAsync()
        {
            await Session.LoadAsync();
        }

        public void Remove(string key)
        {
            Session.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            Session.Set(key, value);
        }

        public void SetObjectAsJson(string key, object value)
        {
            SetString(key, JsonConvert.SerializeObject(value));
        }

        public void SetString(string key, string value)
        {
            Session.SetString(key, value);
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            return Session.TryGetValue(key, out value);
        }

        public void SetInt32(string key, int value)
        {
            Session.SetInt32(key, value);
        }

        public int? GetInt32(string key)
        {
            return Session.GetInt32(key);
        }
    }
}
