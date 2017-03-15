using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Selama.Controllers.Manager;
using Selama.Data.DAL.Core;
using Selama.Data.Models.Core;
using Xunit;

using DataModel = Selama.Data.Models.Core.ResourcePrivilege;
using ViewModel = Selama.Data.ViewModels.Core.ResourcePrivilege;

namespace Selama.Tests.Controllers.Manager
{
    public class PrivilegeControllerUnitTest : ManagerAreaControllerUnitTestBase<PrivilegesController>
    {
        #region Test setup
        protected override PrivilegesController SetupController()
        {
            return new PrivilegesController(_mockPrivilegeDb.Object);
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
            List<ViewModel> privileges = AssertResultIsListOfResourcePrivileges(result);
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
        private List<ViewModel> AssertResultIsListOfResourcePrivileges(JsonResult result)
        {
            Assert.NotNull(result);
            List<ViewModel> privileges = result.Value as List<ViewModel>;
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