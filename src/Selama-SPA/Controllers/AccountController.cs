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
using Selama_SPA.Common;

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
                var appUser = await _userManager.FindByEmailAsync(user.Email);
                if (appUser != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(appUser.UserName, user.Password, user.RememberMe, false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(1, string.Format("User {0} logged in", user.Email));
                        if (user.RememberMe)
                        {
                            _jwtOptions.ValidFor = TimeSpan.FromDays(7);
                        }
                        return await IssueJwt(user.Email);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        // TODO: Implement Two-factor auth
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning(2, string.Format("User {0} locked out", user.Email));
                        // TODO: Implement lock out
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid email or password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                }
            }

            return BadRequestJson(ModelErrorsAsJson());
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<JsonResult> Register([FromBody] RegisterUser user)
        {
            if (ModelState.IsValid)
            {
                var appUser = new ApplicationUser
                {
                    UserName = user.Username,
                    Email = user.Email,
                };
                var result = await _userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded)
                {
                    return await SignIn(new SignInUser 
                    { 
                        Email = user.Email,
                        Password = user.Password,
                        RememberMe = false
                    });
                }
                AddErrors(result);
            }
            return BadRequestJson(ModelErrorsAsJson());
        }

        [HttpPost("sign-out")]
        public async Task<JsonResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Json("");
        }
        #endregion

        #region Private methods
        private async Task<JsonResult> IssueJwt(string emailAddress)
        {
            Claim[] jwtClaims = await CreateJwtClaims(emailAddress);
            string encodedJwt = CreateEncodedJwt(jwtClaims);

            var accessToken = new
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
            };
            return Json(accessToken);
        }
        private async Task<Claim[]> CreateJwtClaims(string emailAddress)
        {
            return new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, emailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JitGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, _jwtOptions.IssuedAt.ToUnixEpochTime().ToString()),
                new Claim(Globals.JWT_CLAIM_TYPE, Globals.JWT_CLAIM_VALUE)
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        #endregion
        #endregion
    }
}