using Guilded.Controllers.Manager;
using Guilded.Data.DAL.Core;
using Guilded.Data.ViewModels.Core;
using Guilded.Extensions;
using Guilded.Security.Claims;
using Moq;
using System.Collections.Generic;

namespace Guilded.Tests.Controllers.Manager
{
    public abstract class ManagerAreaControllerUnitTestBase<TController> : ApiControllerUnitTestBase<TController>
        where TController : ManagerControllerBase
    {
        #region Properties
        #region Protected Properties
        protected const int NUM_PRIVILEGES = 5;

        protected readonly List<Permission> _permissions = new List<Permission>();
        protected readonly Mock<IManagerDataContext> _mockPrivilegeDb = new Mock<IManagerDataContext>(); 
        #endregion
        #endregion

        #region Test setup
        protected override void AdditionalSetup()
        {
            _permissions.AddRange(RoleClaimTypes.RoleClaims.ToListOfDifferentType(rc => new Permission(rc)));
            _mockPrivilegeDb.Setup(db => db.Permissions.Get()).Returns(_permissions);
        }
        #endregion
    }
}