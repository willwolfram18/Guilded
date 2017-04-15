using Guilded.AutoMapper;

namespace Guilded.Tests
{
    public abstract class UnitTestBase
    {
        protected UnitTestBase()
        {
            Mappings.Initialize();
        }
    }
}
