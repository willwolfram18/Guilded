using Microsoft.AspNetCore.Mvc;

namespace Guilded.Tests.Controllers
{
    public abstract class ControllerTestBase<TController>
        where TController : Controller
    {
        protected readonly TController Controller;

        protected abstract TController SetUpController();

        protected ControllerTestBase()
        {
            Controller = SetUpController();
        }
    }
}