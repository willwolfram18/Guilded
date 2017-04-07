using Guilded.Controllers.Admin;
using Guilded.Data.DAL.Core;
using Guilded.ViewModels.Core;
using Guilded.Extensions;
using Guilded.Security.Claims;
using Moq;
using System.Collections.Generic;

namespace Guilded.Tests.Controllers.Admin
{
    public abstract class AdminAreaControllerUnitTestBase<TController> : ApiControllerUnitTestBase<TController>
        where TController : AdminControllerBase
    {
        #region Properties
        #region Protected Properties
        protected const int NUM_PRIVILEGES = 5;

        protected readonly List<Permission> _permissions = new List<Permission>();
        protected readonly Mock<IAdminDataContext> _mockAdminContext = new Mock<IAdminDataContext>(); 
        #endregion
        #endregion

        #region Test setup
        protected override void AdditionalSetup()
        {
            _permissions.AddRange(RoleClaimTypes.RoleClaims.ToListOfDifferentType(rc => new Permission(rc)));
            _mockAdminContext.Setup(db => db.GetPermissions()).Returns(_permissions);
        }
        #endregion
    }
}