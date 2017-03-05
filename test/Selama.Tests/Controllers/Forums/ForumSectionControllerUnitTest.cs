using System;
using Moq;
using Selama_SPA.Controllers.Forums;
using Selama_SPA.Data.DAL.Forums;

namespace Selama.Tests.Controllers.Forums
{
    public class ForumSectionControllerUnitTest : ControllerUnitTestBase<ForumSectionController>
    {
        #region Properties
        #region Private Properties
        private readonly Mock<IForumsReadWriteDataContext> forumsDataContext = new Mock<IForumsReadWriteDataContext>();
        #endregion
        #endregion

        #region Test init
        protected override ForumSectionController SetupController()
        {
            return new ForumSectionController(this.forumsDataContext.Object);
        }
        #endregion
    }
}