using Microsoft.AspNetCore.Mvc;
using Selama.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Selama.Tests.Controllers
{
    public class HomeControllerUnitTests : ControllerUnitTestBase<HomeController>
    {
        [Fact]
        public void IndexIsNotNull()
        {
            #region Arrange
            #endregion

            #region Act
            ViewResult result = Controller.Index();
            #endregion

            #region Assert
            Assert.NotNull(result);
            Assert.Equal(Session.GetString("Sample"), "Meep");
            #endregion
        }
    }
}
