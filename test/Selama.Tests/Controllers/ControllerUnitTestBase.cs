using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using Selama.Controllers;
using Selama.Tests.Common.Mocking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Selama.Tests.Controllers
{
    public abstract class ControllerUnitTestBase<TController>
        where TController : _ControllerBase
    {
        #region Properties
        #region Protected properties
        protected TController Controller { get; private set; }

        protected Mock<ControllerContext> MockControllerContext { get; private set; }

        protected Mock<ActionContext> MockActionContext { get; private set; }

        protected Mock<RouteData> MockRouteData { get; private set; }

        protected Mock<ControllerActionDescriptor> MockActionDescriptor { get; private set; }

        protected Mock<HttpContext> MockHttpContext { get; private set; }

        protected Mock<HttpResponse> MockHttpResponse { get; private set; }

        protected InMemorySession Session { get; private set; }

        protected Mock<ClaimsPrincipal> MockUser { get; private set; }
        #endregion
        #endregion

        #region Constructors
        public ControllerUnitTestBase()
        {
            InitializeHttpContext();

            InitializeControllerContext();

            Controller = SetupController();
            Controller.ControllerContext = MockControllerContext.Object;
            AdditionalSetup();
        }
        #endregion

        #region Methods
        #region Protected methods
        protected InMemorySession SetupSession()
        {
            return new InMemorySession();
        }

        protected virtual Mock<ClaimsPrincipal> SetupUser()
        {
            return new Mock<ClaimsPrincipal>();
        }

        protected virtual Mock<HttpContext> SetupHttpContext()
        {
            return new Mock<HttpContext>();
        }

        protected virtual Mock<HttpResponse> SetupHttpResponse()
        {
            return new Mock<HttpResponse>();
        }

        protected virtual Mock<RouteData> SetupRouteData()
        {
            return new Mock<RouteData>();
        }

        protected virtual Mock<ControllerActionDescriptor> SetupActionDescriptor()
        {
            return new Mock<ControllerActionDescriptor>();
        }

        protected abstract TController SetupController();

        protected virtual void AdditionalSetup()
        {
            // Nothing to do in base class
        }
        #endregion

        #region Private methods
        private void InitializeHttpContext()
        {
            InitializeHttpContextProperties();

            MapHttpContextPropertiesToMockObjects();
        }
        private void InitializeHttpContextProperties()
        {
            Session = SetupSession();
            MockUser = SetupUser();
            MockHttpContext = SetupHttpContext();
            MockHttpResponse = SetupHttpResponse();
        }
        private void MapHttpContextPropertiesToMockObjects()
        {
            MockHttpContext.Setup(ctxt => ctxt.Session).Returns(Session);
            MockHttpContext.Setup(ctxt => ctxt.User).Returns(MockUser.Object);
            MockHttpContext.Setup(ctxt => ctxt.Response).Returns(MockHttpResponse.Object);
        }

        private void InitializeControllerContext()
        {
            InitializeActionContext();
            MockControllerContext = SetupControllerContext();
        }
        private void InitializeActionContext()
        {
            MockRouteData = SetupRouteData();
            MockActionDescriptor = SetupActionDescriptor();
            MockActionContext = SetupActionContext();
        }
        private Mock<ActionContext> SetupActionContext()
        {
            return new Mock<ActionContext>(MockHttpContext.Object, MockRouteData.Object, MockActionDescriptor.Object);
        }
        private Mock<ControllerContext> SetupControllerContext()
        {
            return new Mock<ControllerContext>(MockActionContext.Object);
        }
        #endregion
        #endregion
    }

    public abstract class ApiControllerUnitTestBase<TController>
        where TController : Controller
    {
        #region Properties
        #region Protected properties
        protected TController Controller { get; private set; }

        protected Mock<ControllerContext> MockControllerContext { get; private set; }

        protected Mock<ActionContext> MockActionContext { get; private set; }

        protected Mock<RouteData> MockRouteData { get; private set; }

        protected Mock<ControllerActionDescriptor> MockActionDescriptor { get; private set; }

        protected Mock<HttpContext> MockHttpContext { get; private set; }

        protected Mock<HttpResponse> MockHttpResponse { get; private set; }

        protected InMemorySession Session { get; private set; }

        protected Mock<ClaimsPrincipal> MockUser { get; private set; }
        #endregion
        #endregion

        #region Constructors
        public ApiControllerUnitTestBase()
        {
            InitializeHttpContext();

            InitializeControllerContext();

            Controller = SetupController();
            Controller.ControllerContext = MockControllerContext.Object;
            AdditionalSetup();
        }
        #endregion

        #region Methods
        #region Protected methods
        protected InMemorySession SetupSession()
        {
            return new InMemorySession();
        }

        protected virtual Mock<ClaimsPrincipal> SetupUser()
        {
            return new Mock<ClaimsPrincipal>();
        }

        protected virtual Mock<HttpContext> SetupHttpContext()
        {
            return new Mock<HttpContext>();
        }

        protected virtual Mock<HttpResponse> SetupHttpResponse()
        {
            return new Mock<HttpResponse>();
        }

        protected virtual Mock<RouteData> SetupRouteData()
        {
            return new Mock<RouteData>();
        }

        protected virtual Mock<ControllerActionDescriptor> SetupActionDescriptor()
        {
            return new Mock<ControllerActionDescriptor>();
        }

        protected abstract TController SetupController();

        protected virtual void AdditionalSetup()
        {
            // Nothing to do in base class
        }
        #endregion

        #region Private methods
        private void InitializeHttpContext()
        {
            InitializeHttpContextProperties();

            MapHttpContextPropertiesToMockObjects();
        }
        private void InitializeHttpContextProperties()
        {
            Session = SetupSession();
            MockUser = SetupUser();
            MockHttpContext = SetupHttpContext();
            MockHttpResponse = SetupHttpResponse();
        }
        private void MapHttpContextPropertiesToMockObjects()
        {
            MockHttpContext.Setup(ctxt => ctxt.Session).Returns(Session);
            MockHttpContext.Setup(ctxt => ctxt.User).Returns(MockUser.Object);
            MockHttpContext.Setup(ctxt => ctxt.Response).Returns(MockHttpResponse.Object);
        }

        private void InitializeControllerContext()
        {
            InitializeActionContext();
            MockControllerContext = SetupControllerContext();
        }
        private void InitializeActionContext()
        {
            MockRouteData = SetupRouteData();
            MockActionDescriptor = SetupActionDescriptor();
            MockActionContext = SetupActionContext();
        }
        private Mock<ActionContext> SetupActionContext()
        {
            return new Mock<ActionContext>(MockHttpContext.Object, MockRouteData.Object, MockActionDescriptor.Object);
        }
        private Mock<ControllerContext> SetupControllerContext()
        {
            return new Mock<ControllerContext>(MockActionContext.Object);
        }
        #endregion
        #endregion
    }
}
