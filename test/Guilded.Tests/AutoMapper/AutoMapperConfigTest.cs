using AutoMapper;
using Guilded.AutoMapper;
using Xunit;

namespace Guilded.Tests.AutoMapper
{
    public class AutoMapperConfigTest
    {
        [Fact]
        public void AutoMapperConfigIsCorrect() {
            #region Arrange
            Mappings.Initialize();
            #endregion
        
            #region Act
            Mapper.AssertConfigurationIsValid();
            #endregion
        
            #region Assert
            #endregion
        }
    }
}