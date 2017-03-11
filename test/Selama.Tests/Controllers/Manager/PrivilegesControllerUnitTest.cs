using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Selama_SPA.Controllers.Manager;
using Selama_SPA.Data.DAL.Core;
using Selama_SPA.Data.Models.Core;
using Xunit;

using DataModel = Selama_SPA.Data.Models.Core.ResourcePrivilege;
using ViewModel = Selama_SPA.Data.ViewModels.Core.ResourcePrivilege;

namespace Selama.Tests.Controllers.Manager
{
    public class PrivilegeControllerUnitTest : ApiControllerUnitTestBase<PrivilegesController>
    {
        #region Properties
        #region Private Properties
        private const int NUM_PRIVILEGES = 5;

        private readonly List<DataModel> _privileges = new List<ResourcePrivilege>();
        private readonly Mock<IPrivilegeReadWriteDataContext> _mockPrivilegeDb = new Mock<IPrivilegeReadWriteDataContext>(); 
        #endregion
        #endregion

        #region Test setup
        protected override PrivilegesController SetupController()
        {
            return new PrivilegesController(_mockPrivilegeDb.Object);
        }

        protected override void AdditionalSetup()
        {
            CreatePrivileges();
            _mockPrivilegeDb.Setup(r => r.Privileges.Get())
                .Returns(_privileges.AsQueryable());
            _mockPrivilegeDb.Setup(r => r.Privileges.GetByIdAsync(It.IsAny<int>()))
                .Returns((Func<int, Task<ResourcePrivilege>>)(i => 
                Task.FromResult(_privileges.FirstOrDefault(p => p.Id == i))
            ));
        }
        private void CreatePrivileges()
        {
            for (int i = 0; i < NUM_PRIVILEGES; i++)
            {
                _privileges.Add(new DataModel
                {
                    Id = i + 1,
                    Name = "Privilege " + (i + 1).ToString(),
                });
            }
        }
        #endregion

        #region Methods
        #region Unit Tests
        [Fact]
        public void Get_CorrectList()
        {
            #region Arrange
            #endregion
        
            #region Act
            JsonResult result = Controller.Get();
            #endregion
        
            #region Assert
            List<ResourcePrivilege> privileges = AssertResultIsListOfResourcePrivileges(result);
            Assert.Equal(_privileges.Count, privileges.Count);
            for (int i = 0; i < _privileges.Count; i++)
            {
                Assert.Equal(_privileges[i].Id, privileges[i].Id);
                Assert.Equal(_privileges[i].Name, privileges[i].Name);
            }
            #endregion
        }

        #region PrivilegesController.Get(int)
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task Get_ValidIdCorrectPrivilege(int expectedPrivilegeId)
        {
            #region Arrange
            DataModel privilege = _privileges.FirstOrDefault(p => p.Id == expectedPrivilegeId);
            #endregion
        
            #region Act
            JsonResult result = await Controller.Get(expectedPrivilegeId);
            #endregion
        
            #region Assert
            ViewModel receivedPrivilege = AssertResultIsResourcePrivilege(result);
            Assert.Equal(privilege.Id, receivedPrivilege.Id);
            Assert.Equal(privilege.Name, receivedPrivilege.Name);
            #endregion
        }

        [Theory]
        [InlineData(0)]
        [InlineData(NUM_PRIVILEGES + 1)]
        public async Task Get_InvalidIdReturnsNull(int invalidId)
        {
            #region Arrange
            #endregion
        
            #region Act
            JsonResult result = await Controller.Get(invalidId);
            #endregion
        
            #region Assert
            Assert.NotNull(result);
            Assert.Null(result.Value);
            #endregion
        }
        #endregion
        #endregion

        #region Private Methods
        private List<ResourcePrivilege> AssertResultIsListOfResourcePrivileges(JsonResult result)
        {
            Assert.NotNull(result);
            List<ResourcePrivilege> privileges = result.Value as List<ResourcePrivilege>;
            Assert.NotNull(privileges);
            return privileges;
        }

        private ViewModel AssertResultIsResourcePrivilege(JsonResult result)
        {
            Assert.NotNull(result);
            ViewModel privilege = result.Value as ViewModel;
            Assert.NotNull(privilege);
            return privilege;
        }
        #endregion
        #endregion
    }
}