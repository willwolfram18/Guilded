using System;
using Selama_SPA.Controllers.Manager;

namespace Selama.Tests.Controllers.Manager
{
    public class RolesControllerUnitTest : ManagerAreaControllerUnitTestBase<RolesController>
    {
        #region Test setup
        protected override RolesController SetupController()
        {
            return new RolesController(_mockPrivilegeDb.Object);
        }

        protected override void AdditionalSetup()
        {
            base.AdditionalSetup();
        }
        #endregion
    }
}