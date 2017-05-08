using Guilded.AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

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

            Mappings.Initialize();
        }
    }
}