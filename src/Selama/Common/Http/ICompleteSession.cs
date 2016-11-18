using Microsoft.AspNetCore.Http;

namespace Selama.Common.Http
{
    /// <summary>
    /// Serves as an intermediary interface for <see cref="ISession"/>
    /// to allow for mocking session information for the extension methods
    /// Set/GetString, Set/GetInt32, and setting/getting complex objects
    /// </summary>
    public interface ICompleteSession : ISession
    {
        void SetObjectAsJson(string key, object value);
        T GetObjectFromJson<T>(string key);

        void SetString(string key, string value);
        string GetString(string key);

        void SetInt32(string key, int value);
        int? GetInt32(string key);
    }
}
