using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Selama.Options
{
    public class JwtOptions
    {
        #region Properties
        #region Public Properties
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
                return _jitGenerator;
            }
        }

        public SigningCredentials SigningCredentials { get; set; }
        #endregion

        #region Private Properties
        private readonly Func<Task<string>> _jitGenerator;
        #endregion
        #endregion

        #region Constructors
        public JwtOptions()
        {
            _jitGenerator = () => Task.FromResult(Guid.NewGuid().ToString());
        }

        public JwtOptions(Func<Task<string>> jitGenerator)
        {
            _jitGenerator = jitGenerator;
        }
        #endregion
    }
}