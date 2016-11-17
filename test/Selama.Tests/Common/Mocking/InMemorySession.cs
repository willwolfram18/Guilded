using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Selama.Common.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Selama.Tests.Common.Mocking
{
    public class InMemorySession : ICompleteSession
    {
        private Guid _id = Guid.NewGuid();
        private Dictionary<string, string> _source = new Dictionary<string, string>();

        public string Id
        {
            get
            {
                return _id.ToString();
            }
        }

        public bool IsAvailable
        {
            get
            {
                return true;
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return _source.Keys;
            }
        }

        public void Clear()
        {
            _source.Clear();
        }

        public Task CommitAsync()
        {
            return Task.FromResult(0);
        }

        public Task LoadAsync()
        {
            return Task.FromResult(0);
        }

        public void Remove(string key)
        {
            _source.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            throw new NotImplementedException();
        }

        public void SetObjectAsJson(string key, object value)
        {
            SetString(key, JsonConvert.SerializeObject(value).ToString());
        }

        public T GetObjectFromJson<T>(string key)
        {
            return JsonConvert.DeserializeObject<T>(GetString(key));
        }

        public void SetString(string key, string value)
        {
            _source[key] = value;
        }

        public string GetString(string key)
        {
            return _source[key];
        }
    }
}
