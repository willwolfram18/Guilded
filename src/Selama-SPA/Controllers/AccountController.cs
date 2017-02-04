using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Selama_SPA.Common.Attributes;
using Selama_SPA.Data.Models.Core;
using Selama_SPA.Data.ViewModels.Account;
using Selama_SPA.Options;
using Selama_SPA.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Selama_SPA.Extensions;
using System.Collections.Generic;

namespace Selama_SPA.Controllers
{
    [ApiRoute("[controller]")]
    public class AccountController : ApiControllerBase
    {
        #region Properties
        #region Private Properties
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        #endregion
        #endregion

        public AccountController(IOptions<JwtOptions> jwtOptions,
            ILoggerFactory loggerFactory,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender)
        {
            _jwtOptions = jwtOptions.Value;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        #region Methods
        #region Action methods
        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<JsonResult> SignIn([FromBody] SignInUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, user.RememberMe, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, string.Format("User {0} logged in", user.Username));
                    if (user.RememberMe)
                    {
                        _jwtOptions.ValidFor = TimeSpan.FromDays(7);
                    }
                    return await IssueJwt(user.Username);
                }
                if (result.RequiresTwoFactor)
                {
                    // TODO: Implement Two-factor auth
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, string.Format("User {0} locked out", user.Username));
                    // TODO: Implement lock out
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }

            return BadRequestJson(ModelErrorsAsJson());
        }

        [AllowAnonymous]
        [HttpPost]
        public Task<IActionResult> Register([FromBody] RegisterUser user)
        {
            throw new NotImplementedException();
        }

        [HttpPost("sign-out")]
        public IActionResult SignOut()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private methods
        private async Task<JsonResult> IssueJwt(string username)
        {
            Claim[] jwtClaims = await CreateJwtClaims(username);
            string encodedJwt = CreateEncodedJwt(jwtClaims);

            var accessToken = new
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
            };
            return Json(accessToken);
        }
        private async Task<Claim[]> CreateJwtClaims(string username)
        {
            return new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JitGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, _jwtOptions.IssuedAt.ToUnixEpochTime().ToString()),
                new Claim("Selama Ashalanore User", username)
            };
        }
        private string CreateEncodedJwt(Claim[] jwtClaims)
        {
            var jwt = new JwtSecurityToken(
                            _jwtOptions.Issuer,
                            _jwtOptions.Audience,
                            jwtClaims,
                            _jwtOptions.NotBefore,
                            _jwtOptions.Expiration,
                            _jwtOptions.SigningCredentials
                        );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        private Dictionary<string, List<string>> ModelErrorsAsJson()
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            foreach (var modelError in ModelState)
            {
                result.Add(modelError.Key, new List<string>());
                foreach (var error in modelError.Value.Errors)
                {
                    result[modelError.Key].Add(error.ErrorMessage);
                }
            }
            return result;
        }
        #endregion
        #endregion
    }
}