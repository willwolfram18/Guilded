using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Selama.Controllers.Manager;
using Selama.Data.DAL.Core;
using Selama.Data.Models.Core;

using DataModel = Selama.Data.Models.Core.ResourcePrivilege;

namespace Selama.Tests.Controllers.Manager
{
    public abstract class ManagerAreaControllerUnitTestBase<TController> : ApiControllerUnitTestBase<TController>
        where TController : ManagerControllerBase
    {
        #region Properties
        #region Protected Properties
        protected const int NUM_PRIVILEGES = 5;

        protected readonly List<DataModel> _privileges = new List<ResourcePrivilege>();
        protected readonly Mock<IPrivilegeReadWriteDataContext> _mockPrivilegeDb = new Mock<IPrivilegeReadWriteDataContext>(); 
        #endregion
        #endregion

        #region Test setup
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
    }
}