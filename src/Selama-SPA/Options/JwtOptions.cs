using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Selama_SPA.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; }

        public string Subject { get; set; }

        public string Audience { get; set; }

        public DateTime NotBefore { get; set; } = DateTime.UtcNow;

        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(60);

        public DateTime Expiration
        {
            get
            {
                return IssuedAt.Add(ValidFor);
            }
        }

        public Func<Task<string>> JitGenerator
        {
            get
            {
                return () => Task.FromResult(Guid.NewGuid().ToString());
            }
        }

        public SigningCredentials SigningCredentials { get; set; }
    }
}