using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Guilded.Tests.Controllers
{
    [TestFixture]
    public abstract class ControllerTestBase<TController>
        where TController : Controller
    {
        protected TController Controller { get; set; }

        protected abstract TController SetUpController();

        [SetUp]
        public void BaseSetUp()
        {
            Controller = SetUpController();
        }
    }
}