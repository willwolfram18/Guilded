using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Guilded.Controllers.Manager;
using Guilded.Data.DAL.Core;
using Guilded.Data.Models.Core;

using DataModel = Guilded.Data.Models.Core.ResourcePrivilege;
using Guilded.Security.Claims;
using Guilded.Data.ViewModels.Core;
using Guilded.Extensions;

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