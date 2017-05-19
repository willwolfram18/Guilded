using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using Guilded.Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Guilded.Tests.Controllers
{
    [TestFixture]
    public abstract class ControllerTest<TController>
        where TController : BaseController
    {
        protected TController Controller { get; private set; }

        protected Mock<IUrlHelper> MockUrlHelper { get; private set; }

        protected Mock<ITempDataDictionary> MockTempData { get; private set; }

        protected Mock<ControllerContext> MockControllerContext { get; private set; }

        protected Mock<ActionContext> MockActionContext { get; private set; }

        protected Mock<RouteData> MockRouteData { get; private set; }

        protected Mock<ControllerActionDescriptor> MockActionDescriptor { get; private set; }

        protected Mock<HttpContext> MockHttpContext { get; private set; }

        protected Mock<HttpRequest> MockHttpRequest { get; private set; }

        protected Mock<HttpResponse> MockHttpResponse { get; private set; }

        protected Mock<ClaimsPrincipal> MockUser { get; private set; }

        protected abstract TController SetUpController();

        [SetUp]
        public void BaseSetUp()
        {
            InitializeHttpContext();

            InitializeControllerContext();

            MockUrlHelper = SetUpUrlHelper();
            MockTempData = SetUpTempData();

            Controller = SetUpController();
            Controller.ControllerContext = MockControllerContext.Object;
            Controller.Url = MockUrlHelper.Object;
            Controller.TempData = MockTempData.Object;

            AdditionalSetUp();
        }

        /// <summary>
        /// Sets up and returns the mocked user for the controller.
        /// </summary>
        /// <returns></returns>
        protected virtual Mock<ClaimsPrincipal> SetUpUser()
        {
            return new Mock<ClaimsPrincipal>();
        }

        /// <summary>
        /// Sets up and returns the mocked <see cref="HttpContext"/> to be used in the tests.
        /// </summary>
        /// <returns></returns>
        protected virtual Mock<HttpContext> SetUpHttpContext()
        {
            return new Mock<HttpContext>();
        }

        /// <summary>
        /// Sets up and returns the mocked <see cref="HttpResponse"/> to be used in the tests.
        /// </summary>
        /// <returns></returns>
        protected virtual Mock<HttpResponse> SetUpHttpResponse()
        {
            return new Mock<HttpResponse>();
        }

        /// <summary>
        /// Sets up and returns the mocked <see cref="HttpRequest"/> to be used in the tests.
        /// </summary>
        /// <returns></returns>
        protected virtual Mock<HttpRequest> SetUpHttpRequest()
        {
            return new Mock<HttpRequest>();
        }

        /// <summary>
        /// Sets up and returns the mocked <see cref="RouteData"/> to be used in the tests.
        /// </summary>
        /// <returns></returns>
        protected virtual Mock<RouteData> SetUpRouteData()
        {
            return new Mock<RouteData>();
        }

        /// <summary>
        /// Sets up and returns the mocked <see cref="ControllerActionDescriptor"/> to be
        /// used in the tests.
        /// </summary>
        /// <returns></returns>
        protected virtual Mock<ControllerActionDescriptor> SetUpActionDescriptor()
        {
            return new Mock<ControllerActionDescriptor>();
        }

        /// <summary>
        /// Sets up and returns the mocked <see cref="IUrlHelper"/> to be used in the tests.
        /// </summary>
        /// <returns></returns>
        protected virtual Mock<IUrlHelper> SetUpUrlHelper()
        {
            return new Mock<IUrlHelper>();
        }

        protected virtual Mock<ITempDataDictionary> SetUpTempData()
        {
            return new Mock<ITempDataDictionary>();
        }

        /// <summary>
        /// Perform anys additional set up necessary for the tests, after the controller is initalized.
        /// </summary>
        protected virtual void AdditionalSetUp()
        {
            // Nothing to do in base class
        }

        private void InitializeHttpContext()
        {
            InitializeHttpContextProperties();

            MapHttpContextPropertiesToMockObjects();
        }

        private void InitializeHttpContextProperties()
        {
            MockUser = SetUpUser();
            MockHttpContext = SetUpHttpContext();
            MockHttpRequest = SetUpHttpRequest();
            MockHttpResponse = SetUpHttpResponse();
            MockHttpResponse.SetupSet(r => r.StatusCode = It.IsAny<int>()).Verifiable();
        }

        private void MapHttpContextPropertiesToMockObjects()
        {
            MockHttpContext.Setup(ctxt => ctxt.User).Returns(MockUser.Object);
            MockHttpContext.Setup(ctxt => ctxt.Request).Returns(MockHttpRequest.Object);
            MockHttpContext.Setup(ctxt => ctxt.Response).Returns(MockHttpResponse.Object);
        }

        private void InitializeControllerContext()
        {
            InitializeActionContext();
            MockControllerContext = SetupControllerContext();
        }

        private void InitializeActionContext()
        {
            MockRouteData = SetUpRouteData();
            MockActionDescriptor = SetUpActionDescriptor();
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
    }
}