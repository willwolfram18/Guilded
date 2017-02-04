using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Selama_SPA.Controllers;
using Selama_SPA.Data.Models.Core;
using Selama_SPA.Options;
using Selama_SPA.Services;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Selama_SPA.Data;
using Selama_SPA.Data.ViewModels.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Selama_SPA.Extensions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace Selama.Tests.Controllers
{
    public class AccountControllerUnitTest : ApiControllerUnitTestBase<AccountController>
    {
        #region Properties
        #region Private Properties
        /// <summary>
        /// Function expression behind <see cref="JwtOptions.JitGenerator"/>
        /// for <see cref="_jwtOptions"/>
        /// </summary>
        private static readonly Func<Task<string>> _jitGenerator = (() => Task.FromResult("JitToken"));
        /// <summary>
        /// Object behind <see cref="_mockJwtOptions"/>
        /// </summary>
        private readonly JwtOptions _jwtOptions = new JwtOptions(_jitGenerator);

        private readonly Mock<IOptions<JwtOptions>> _mockJwtOptions = new Mock<IOptions<JwtOptions>>();
        private readonly Mock<IEmailSender> _mockEmailSender = new Mock<IEmailSender>();
        private readonly Mock<ISmsSender> _mockSmsSender = new Mock<ISmsSender>();

        private IServiceProvider _serviceProvider;
        private ApplicationDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager;
        private Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        #endregion
        #endregion

        #region Test setup
        protected override AccountController SetupController()
        {
            _mockJwtOptions.Setup(jwt => jwt.Value).Returns(_jwtOptions);
            SetupInMemoryDbAndUserManager();

            return new AccountController(_mockJwtOptions.Object,
                _serviceProvider.GetRequiredService<ILoggerFactory>(),
                _userManager,
                _mockSignInManager.Object,
                _mockEmailSender.Object,
                _mockSmsSender.Object
            );
        }
        // Code inspired by http://stackoverflow.com/a/34765902
        private void SetupInMemoryDbAndUserManager()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddOptions();
            AddInMemoryDbServices(services);
            AddHttpContextAccessorService(services);

            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            _userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                _userManager,
                _serviceProvider.GetService<IHttpContextAccessor>(),
                _serviceProvider.GetService<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                _serviceProvider.GetService<IOptions<IdentityOptions>>(),
                _serviceProvider.GetService<ILogger<SignInManager<ApplicationUser>>>()
            );
        }
        private void AddInMemoryDbServices(ServiceCollection services)
        {
            var efServiceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseInMemoryDatabase()
                    .UseInternalServiceProvider(efServiceProvider)
            );
            services.AddIdentity<ApplicationUser, IdentityRole>(opts => 
                {
                    opts.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }
        private void AddHttpContextAccessorService(ServiceCollection services)
        {
            var httpAuthFeature = new HttpAuthenticationFeature();
            MockHttpContext.Setup(ctxt => ctxt.Features.Get<IHttpAuthenticationFeature>())
                .Returns(httpAuthFeature);
            MockHttpContext.Setup(ctxt => ctxt.Features[typeof(IHttpAuthenticationFeature)])
                .Returns(httpAuthFeature);

            services.AddSingleton<IHttpContextAccessor>(h => new HttpContextAccessor { HttpContext = MockHttpContext.Object });
        }
        #endregion

        #region Methods
        #region Unit tests
        #region AccountController.SignIn
        [Fact]
        public async Task SignIn_InvalidModelReturnsBadRequest()
        {
            #region Arrange
            SignInUser user = new SignInUser();
            Controller.ModelState.AddModelError("Email", "Username is required");
            Controller.ModelState.AddModelError("Password", "Password is required");
            #endregion

            #region Act
            JsonResult result = await Controller.SignIn(user);
            #endregion

            #region Assert
            AssertIsBadRequest();
            JObject resultJson = ConvertResultToJson(result);
            Assert.True(resultJson.ContainsKey("Email"));
            Assert.True(resultJson.ContainsKey("Password"));
            #endregion
        }

        [Fact]
        public async Task SignIn_InvalidUserNameReturnsBadRequest()
        {
            #region Arrange
            Tuple<ApplicationUser, string> userPassword = await CreateSampleUser();

            SignInUser user = new SignInUser
            {
                Email = "boop",
                Password = userPassword.Item2
            };
            SignInFails();
            #endregion

            #region Act & Assert
            await AssertInvaludUsernameOrPassword(user);
            #endregion
        }

        [Fact]
        public async Task SignIn_InvalidPasswordReturnsBadRequest()
        {
            #region Arrange
            Tuple<ApplicationUser, string> userPassword = await CreateSampleUser();

            SignInUser user = new SignInUser
            {
                Email = userPassword.Item1.Email,
                Password = "boop"
            };
            SignInFails();
            #endregion

            #region Act & Assert
            await AssertInvaludUsernameOrPassword(user);
            #endregion
        }

        [Fact]
        public async Task SignIn_ProperJwtIssued()
        {
            #region Arrange
            Tuple<ApplicationUser, string> userPassword = await CreateSampleUser();
            SignInUser user = new SignInUser
            {
                Email = userPassword.Item1.Email,
                Password = userPassword.Item2,
                RememberMe = false,
            };
            SignInSucceeds();
            #endregion

            #region Act
            JsonResult result = await Controller.SignIn(user);
            #endregion

            #region Assert
            AssertIsOkRequest();
            JObject jsonResult = ConvertResultToJson(result);
            Assert.True(jsonResult.ContainsKey("access_token"));
            Assert.True(jsonResult.ContainsKey("expires_in"));
            Assert.Equal(_jwtOptions.ValidFor.TotalSeconds, jsonResult["expires_in"].ToObject<double>());
            #endregion
        }

        [Fact]
        public async Task SignIn_RememberMeIssuesSevenDayToken()
        {
            #region Arrange
            Tuple<ApplicationUser, string> userPassword = await CreateSampleUser();
            SignInUser user = new SignInUser
            {
                Email = userPassword.Item1.Email,
                Password = userPassword.Item2,
                RememberMe = true,
            };
            SignInSucceeds();
            #endregion

            #region Act
            JsonResult result = await Controller.SignIn(user);
            #endregion

            #region Assert
            AssertIsOkRequest();
            JObject jsonResult = ConvertResultToJson(result);
            Assert.True(jsonResult.ContainsKey("access_token"));
            Assert.True(jsonResult.ContainsKey("expires_in"));
            Assert.Equal(TimeSpan.FromDays(7).TotalSeconds, jsonResult["expires_in"].ToObject<double>());
            #endregion
        }
        #endregion

        [Fact]
        public async Task SignOut_VerifySignOutCalled()
        {
            #region Arrange
            #endregion

            #region Act
            IActionResult result = await Controller.SignOut();
            #endregion

            #region Assert
            AssertIsOkRequest();
            _mockSignInManager.Verify(s => s.SignOutAsync(), Times.Once());
            #endregion
        }

        #region AccountController.Register
        [Fact]
        public async Task Register_InvalidModelReturnsBadRequest()
        {
            #region Arrange
            RegisterUser user = new RegisterUser
            {
                Email = "test@example.com",
                Password = "1234@Abc",
                Username = "Sample.User",
            };
            Controller.ModelState.AddModelError("Email", "Email address is required");
            Controller.ModelState.AddModelError("Password", "Password is required");
            Controller.ModelState.AddModelError("Username", "Username is required");
            #endregion

            #region Act
            JsonResult result = await Controller.Register(user);
            #endregion

            #region Assert
            AssertIsBadRequest();
            JObject jsonResult = ConvertResultToJson(result);
            Assert.True(jsonResult.ContainsKey("Email"));
            Assert.True(jsonResult.ContainsKey("Password"));
            Assert.True(jsonResult.ContainsKey("Username"));
            #endregion
        }

        [Fact]
        public async Task Register_EmailAddressInUse()
        {
            #region Arrange
            var sampleUser = await CreateSampleUser();
            RegisterUser user = new RegisterUser
            {
                Email = sampleUser.Item1.Email,
                Username = "Boop",
                Password = sampleUser.Item2,
            };
            #endregion
        
            #region Act
            JsonResult result = await Controller.Register(user);
            #endregion
        
            #region Assert
            AssertIsBadRequest();
            JObject jsonResult = ConvertResultToJson(result);
            Assert.True(jsonResult.ContainsKey(""));
            List<string> errors = GetPropertyErrors(jsonResult, "");
            Assert.Equal(1, errors.Count);
            Assert.Equal($"Email '{user.Email}' is already taken.", errors[0]);
            #endregion
        }

        [Fact]
        public async Task Register_PasswordInvalid()
        {
            #region Arrange
            RegisterUser user = new RegisterUser
            {
                Email = "test@example.com",
                Username = "Boop",
                Password = "abc",
            };
            #endregion
        
            #region Act
            JsonResult result = await Controller.Register(user);
            #endregion
        
            #region Assert
            AssertIsBadRequest();
            JObject jsonResult = ConvertResultToJson(result);
            Assert.True(jsonResult.ContainsKey(""));
            List<string> errors = GetPropertyErrors(jsonResult, "");
            Assert.True(errors.Count > 0);
            #endregion
        }
        #endregion
        #endregion

        #region Private methods
        private async Task<Tuple<ApplicationUser, string>> CreateSampleUser()
        {
            ApplicationUser sampleUser = new ApplicationUser
            {
                UserName = "Sample.User",
                Email = "test@example.com",
            };
            string samplePassword = "1234&Abc";
            var t = await _userManager.CreateAsync(sampleUser, samplePassword);
            Tuple<ApplicationUser, string> userPassword = new Tuple<ApplicationUser, string>(sampleUser, samplePassword);
            return userPassword;
        }
        private async Task AssertInvaludUsernameOrPassword(SignInUser user)
        {
            #region Act
            JsonResult result = await Controller.SignIn(user);
            #endregion

            #region Assert
            AssertIsBadRequest();
            JObject resultJson = ConvertResultToJson(result);
            List<string> modelErrors = GetPropertyErrors(resultJson, "");
            Assert.Equal(1, modelErrors.Count);
            Assert.Equal("Invalid email or password", modelErrors[0]);
            #endregion
        }
        private void SignInFails()
        {
            _mockSignInManager.Setup(s => s.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()
            )).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);
        }
        private void SignInSucceeds()
        {
            _mockSignInManager.Setup(s => s.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()
            )).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        }
        #endregion
        #endregion
    }
}