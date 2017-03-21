using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Guilded.Controllers.Manager;
using Guilded.Data.DAL.Core;
using Guilded.Data.Models.Core;

using DataModel = Guilded.Data.Models.Core.ResourcePrivilege;

namespace Guilded.Tests.Controllers.Manager
{
    public abstract class ManagerAreaControllerUnitTestBase<TController> : ApiControllerUnitTestBase<TController>
        where TController : ManagerControllerBase
    {
        #region Properties
        #region Protected Properties
        protected const int NUM_PRIVILEGES = 5;

        protected readonly List<DataModel> _privileges = new List<ResourcePrivilege>();
        protected readonly Mock<IManagerDataContext> _mockPrivilegeDb = new Mock<IManagerDataContext>(); 
        #endregion
        #endregion

        #region Test setup
        protected override void AdditionalSetup()
        {
            CreatePrivileges();
            _mockPrivilegeDb.Setup(r => r.Permissions.Get())
                .Returns(_privileges.AsQueryable());
            _mockPrivilegeDb.Setup(r => r.Permissions.GetByIdAsync(It.IsAny<int>()))
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
    }
}