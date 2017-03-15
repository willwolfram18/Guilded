using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Selama_SPA.Controllers.Manager;
using Selama_SPA.Data;
using Selama_SPA.Data.Models.Core;

using ViewModel = Selama_SPA.Data.ViewModels.Core.ApplicationRole;
using DataModel = Selama_SPA.Data.Models.Core.ApplicationRole;
using Selama_SPA.Extensions;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Selama_SPA.Data.ViewModels.Core;

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
            List<DataModel> expectedRoles = _roles.OrderBy(r => r.Name)
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
                AssertDataModelMatchesViewModel(expectedRoles[i], roles[i]);
            }
            #endregion
        }

        #region RolesController.Get(string)
        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        [InlineData("4")]
        [InlineData("5")]
        public async Task Get_ValidIdIsCorrectRole(string roleId)
        {
            #region Arrange
            DataModel expectedRole = _roles.FirstOrDefault(r => r.Id == roleId);
            #endregion

            #region Act
            JsonResult result = await Controller.Get(roleId);
            #endregion

            #region Assert
            ViewModel role = AssertResultIsRole(result);
            AssertDataModelMatchesViewModel(expectedRole, role);
            #endregion
        }

        [Theory]
        [InlineData(0)]
        [InlineData(NUM_ROLES + 1)]
        public async Task Get_InvalidIdIsNullRole(int roleIdAsNumber)
        {
            #region Arrange
            #endregion

            #region Act
            JsonResult result = await Controller.Get(roleIdAsNumber.ToString());
            #endregion

            #region Assert
            Assert.NotNull(result);
            Assert.Null(result.Value);
            #endregion
        }
        #endregion
        #endregion

        #region Private methods
        private List<ViewModel> AssertResultIsListOfRoles(JsonResult result)
        {
            Assert.NotNull(result);
            IEnumerable<ViewModel> roles = result.Value as IEnumerable<ViewModel>;
            Assert.NotNull(roles);
            return roles.ToList();
        }

        private ViewModel AssertResultIsRole(JsonResult result)
        {
            Assert.NotNull(result);
            ViewModel role = result.Value as ViewModel;
            Assert.NotNull(role);
            return role;
        }
        private void AssertDataModelMatchesViewModel(DataModel expected, ViewModel actual)
        {
            if (expected == null)
            {
                Assert.Null(actual);
            }
            else
            {
                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Name, actual.Name);
            }
        }
        #endregion
        #endregion
    }
}