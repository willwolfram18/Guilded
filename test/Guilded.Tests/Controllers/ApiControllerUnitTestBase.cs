using Guilded.Controllers;
using Guilded.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using Xunit;

namespace Guilded.Tests.Controllers
{
    public abstract class ApiControllerUnitTestBase<TController>
        where TController : ApiControllerBase
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

        protected void AssertIsOkRequest()
        {
            MockHttpResponse.VerifySet(r => r.StatusCode = It.IsAny<int>(), Times.Never());
        }
        protected void AssertIsBadRequest()
        {
            MockHttpResponse.VerifySet(r => r.StatusCode = It.Is<int>(i =>
                i == (int)HttpStatusCode.BadRequest
            ));
        }
        protected JObject ConvertResultToJson(JsonResult result)
        {
            return JObject.FromObject(result.Value);
        }
        protected static List<string> GetPropertyErrors(JObject resultJson, string propertyName)
        {
            Assert.True((resultJson as IDictionary<string, JToken>).ContainsKey(""));
            List<string> modelErrors = resultJson.GetValue(propertyName).ToObject<List<string>>();
            Assert.NotNull(modelErrors);
            return modelErrors;
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
            MockUser = SetupUser();
            MockHttpContext = SetupHttpContext();
            MockHttpResponse = SetupHttpResponse();
            MockHttpResponse.SetupSet(r => r.StatusCode = It.IsAny<int>()).Verifiable();
        }
        private void MapHttpContextPropertiesToMockObjects()
        {
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
