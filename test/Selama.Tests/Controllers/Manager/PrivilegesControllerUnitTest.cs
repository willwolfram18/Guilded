using System;
using Moq;
using Selama_SPA.Controllers.Manager;
using Selama_SPA.Data.DAL.Core;

namespace Selama.Tests.Controllers.Manager
{
    public class PrivilegeControllerUnitTest : ApiControllerUnitTestBase<PrivilegesController>
    {
        #region Properties
        #region Private Properties
        private readonly Mock<IPrivilegeReadWriteDataContext> _mockPrivilegeDb = new Mock<IPrivilegeReadWriteDataContext>(); 
        #endregion
        #endregion

        protected override PrivilegesController SetupController()
        {
            return new PrivilegesController(_mockPrivilegeDb.Object);
        }
    }
}