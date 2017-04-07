using Guilded.Controllers.Admin;
using Guilded.Data;
using Guilded.Data.DAL.Core;
using Guilded.ViewModels.Core;
using Guilded.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Xunit;
using DataModel = Guilded.Identity.ApplicationRole;
using ViewModel = Guilded.ViewModels.Core.ApplicationRole;

namespace Guilded.Tests.Controllers.Admin
{
    public class RolesControllerUnitTest : AdminAreaControllerUnitTestBase<RolesController>
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
            _mockAdminContext.Setup(db => db.GetRoles()).Returns(_roleManager.Roles);
            _mockAdminContext.Setup(db => db.GetRoleById(It.IsAny<string>()))
                .Returns((Func<string, DataModel>)(id => _roleManager.Roles.FirstOrDefault(r => r.Id == id)));
            return new RolesController(_mockAdminContext.Object);
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
                .AddRoleManager<ApplicationRoleManager>()
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
            int identityRoleClaimCounter = 1;
            for (int i = 0; i < NUM_ROLES; i++)
            {
                string roleId = (i + 1).ToString();
                var newRole = new DataModel
                {
                    Id = roleId,
                    Name = "Role " + roleId,
                    ConcurrencyStamp = roleId,
                };
                newRole.Claims.Add(new IdentityRoleClaim<string>
                {
                    Id = identityRoleClaimCounter++,
                    RoleId = (i + 1).ToString(),
                    ClaimType = _permissions[i].PermissionType,
                    ClaimValue = true.ToString(),
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
            List<ViewModel> roles = AssertResultIsListOfRoleViewModels(result);
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
            ViewModel role = AssertResultIsRoleViewModel(result);
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

        #region RoleController.CreateOrUpdate(ViewModel)
        [Fact]
        public void CreateOrUpdate_InvalidIdCreatesNewRole() {
            #region Arrange
            string roleId = (NUM_ROLES + 1).ToString();
            ViewModel roleToCreate = new ViewModel()
            {
                Id = roleId,
                Name = "New Role",
            };
            #endregion
        
            #region Act
            var result = Controller.CreateOrUpdate(roleToCreate);
            #endregion
        
            #region Assert
            var createdRole = AssertResultIsRoleViewModel(result);
            _mockAdminContext.Verify(db => db.GetRoleById(It.Is<string>(id => id == roleId)), Times.Once());
            _mockAdminContext.Verify(db => db.CreateRoleAsync(
                It.Is<string>(name => name == roleToCreate.Name),
                It.IsAny<IEnumerable<Permission>>()
            ), Times.Once());
            _mockAdminContext.Verify(db => db.UpdateRoleAsync(It.IsAny<DataModel>()), Times.Never());
            #endregion
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        [InlineData("4")]
        [InlineData("5")]
        public void CreateOrUpdate_ValidIdAndMatchingConcurrencyStampUpdatesExistingRole(string roleId) {
            #region Arrange
            var roleToUpdate = new ViewModel
            {
                Id = roleId,
                Name = "Updated Role Name",
                ConcurrencyStamp = roleId,
                Permissions = new List<Permission>(),
            };
            _mockAdminContext.Setup(db => db.UpdateRoleAsync(It.IsAny<DataModel>()))
                .Returns(Task.FromResult(Mapper.Map<DataModel>(roleToUpdate)));
            #endregion

            #region Act
            var result = Controller.CreateOrUpdate(roleToUpdate);
            #endregion

            #region Assert
            ViewModel updatedRole = AssertResultIsRoleViewModel(result);
            Assert.NotNull(updatedRole);
            _mockAdminContext.Verify(db => db.UpdateRoleAsync(It.Is<DataModel>(r => r.Id == roleId)), Times.Once());
            _mockAdminContext.Verify(db => db.CreateRoleAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<Permission>>()
            ), Times.Never());
            AssertViewModelsMatch(roleToUpdate, updatedRole);
            #endregion
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        [InlineData("4")]
        [InlineData("5")]
        public void CreateOrUpdate_ValidIdWithInvalidConcurrencyStampReturnsCurrentRole(string roleId)
        {
            #region Arrange

            #endregion

            #region Act

            #endregion

            #region Assert

            #endregion
        }
        #endregion
        #endregion

        #region Private methods
        private List<ViewModel> AssertResultIsListOfRoleViewModels(JsonResult result)
        {
            Assert.NotNull(result);
            IEnumerable<ViewModel> roles = result.Value as IEnumerable<ViewModel>;
            Assert.NotNull(roles);
            return roles.ToList();
        }

        private ViewModel AssertResultIsRoleViewModel(JsonResult result)
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
        private void AssertViewModelsMatch(ViewModel expected, ViewModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.ConcurrencyStamp, actual.ConcurrencyStamp);
            Assert.Equal(expected.Permissions.Count, actual.Permissions.Count);

            var expectedPermissions = expected.Permissions.OrderBy(p => p.PermissionType).ToList();
            var actualPermissions = actual.Permissions.OrderBy(p => p.PermissionType).ToList();
            for (int i = 0; i < expectedPermissions.Count; i++)
            {
                Assert.Equal(expectedPermissions[i].PermissionType, actualPermissions[i].PermissionType);
            }
        }
        #endregion
        #endregion
    }
}