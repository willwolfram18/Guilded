using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Selama_SPA.Controllers.Manager;
using Selama_SPA.Data;
using Selama_SPA.Data.Models.Core;

namespace Selama.Tests.Controllers.Manager
{
    public class RolesControllerUnitTest : ManagerAreaControllerUnitTestBase<RolesController>
    {
        #region Properties
        #region Private Properties
        private const int NUM_ROLES = 5;

        private IServiceProvider _serviceProvider;
        private ApplicationDbContext _dbContext;
        private RoleManager<ApplicationRole> _roleManager;
        private readonly List<ApplicationRole> _roles = new List<ApplicationRole>();
        #endregion
        #endregion

        #region Test setup
        protected override RolesController SetupController()
        {
            SetupInMemoryDbAndUserManager();
            _mockPrivilegeDb.Setup(db => db.RoleManager).Returns(_roleManager);
            return new RolesController(_mockPrivilegeDb.Object);
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
            _roleManager = _serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        }
        private void AddInMemoryDbServices(ServiceCollection services)
        {
            var efServiceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseInMemoryDatabase()
                    .UseInternalServiceProvider(efServiceProvider)
            );
            services.AddIdentity<ApplicationUser, ApplicationRole>(opts => 
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

        protected override void AdditionalSetup()
        {
            base.AdditionalSetup();
            CreateRoles().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        private async Task CreateRoles()
        {
            for (int i = 0; i < NUM_ROLES; i++)
            {
                var newRole = new ApplicationRole
                {
                    Id = (i + 1).ToString(),
                    Name = "Role " + (i + 1).ToString(),
                };
                newRole.RolePrivileges.Add(new RolePrivilege
                {
                    RoleId = (i + 1).ToString(),
                    Role = newRole,
                    PrivilegeId = (i % NUM_PRIVILEGES) + 1,
                    Privilege = _privileges.FirstOrDefault(p => p.Id == (i % NUM_PRIVILEGES) + 1),
                });
                _roles.Add(newRole);
                _roleManager.CreateAsync(newRole).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
        #endregion
    }
}