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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Http;

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
        private SignInManager<ApplicationUser> _signInManager;
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
                _signInManager,
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
            _signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
        }
        private void AddInMemoryDbServices(ServiceCollection services)
        {
            var efServiceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseInMemoryDatabase()
                    .UseInternalServiceProvider(efServiceProvider)
            );
            services.AddIdentity<ApplicationUser, IdentityRole>()
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
        #region Private methods
        [Fact]
        public void Testm()
        {
            Assert.True(true);
        }
        #endregion
        #endregion
    }
}