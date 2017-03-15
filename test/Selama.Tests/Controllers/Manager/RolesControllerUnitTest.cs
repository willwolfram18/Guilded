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

using ViewModel = Selama_SPA.Data.ViewModels.Core.ApplicationRole;
using DataModel = Selama_SPA.Data.Models.Core.ApplicationRole;
using Selama_SPA.Extensions;
using Microsoft.AspNetCore.Mvc;
using Selama_SPA.Data.ViewModels.Core;
using Xunit;

namespace Selama.Tests.Controllers.Manager
{
    public class RolesControllerUnitTest : ManagerAreaControllerUnitTestBase<RolesController>
    {
        #region Properties
        #region Private Properties
        private const int NUM_ROLES = 5;

        private IServiceProvider _serviceProvider;
        private ApplicationDbContext _dbContext;
        private RoleManager<DataModel> _roleManager;
        private readonly List<DataModel> _roles = new List<DataModel>();
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
            _roleManager = _serviceProvider.GetRequiredService<RoleManager<DataModel>>();
        }
        private void AddInMemoryDbServices(ServiceCollection services)
        {
            var efServiceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseInMemoryDatabase()
                    .UseInternalServiceProvider(efServiceProvider)
            );
            services.AddIdentity<ApplicationUser, DataModel>(opts =>
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
        private Task CreateRoles()
        {
            for (int i = 0; i < NUM_ROLES; i++)
            {
                var newRole = new DataModel
                {
                    Id = (i + 1).ToString(),
                    Name = "Role " + (i + 1).ToString(),
                    RolePrivileges = new List<RolePrivilege>(),
                };
                int associatedPrivilegeId = (i % NUM_PRIVILEGES) + 1;
                newRole.RolePrivileges.Add(new RolePrivilege
                {
                    RoleId = (i + 1).ToString(),
                    Role = newRole,
                    PrivilegeId = associatedPrivilegeId,
                    Privilege = _privileges.FirstOrDefault(p => p.Id == associatedPrivilegeId),
                });
                _roles.Add(newRole);
                _roleManager.CreateAsync(newRole).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            return Task.CompletedTask;
        }
        #endregion

        #region Methods
        #region Unit tests
        [Fact]
        public void Get_CorrectList()
        {
            #region Arrange
            List<ViewModel> expectedRoles = _roles.ToListOfDifferentType(r => new ViewModel(r))
                .OrderBy(r => r.Name)
                .ToList();
            #endregion

            #region Act
            JsonResult result = Controller.Get();
            #endregion

            #region Assert
            List<ViewModel> roles = AssertResultIsListOfRoles(result);
            Assert.Equal(expectedRoles.Count, roles.Count);
            for (int i = 0; i < expectedRoles.Count; i++)
            {
                Assert.Equal(expectedRoles[i].Id, roles[i].Id);
                Assert.Equal(expectedRoles[i].Name, roles[i].Name);
            }
            #endregion
        }
        #endregion

        #region Private methods
        private List<ViewModel> AssertResultIsListOfRoles(JsonResult result)
        {
            Assert.NotNull(result);
            IEnumerable<ViewModel> roles = result.Value as IEnumerable<ViewModel>;
            Assert.NotNull(roles);
            return roles.ToList();
        }
        #endregion
        #endregion
    }
}