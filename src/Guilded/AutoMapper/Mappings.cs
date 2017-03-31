using AutoMapper;

namespace Guilded.AutoMapper
{
    public static class Mappings
    {
        public static void Initialize()
        {
            Mapper.Initialize(config => {
                config.AddProfile<IdentityMappingProfile>();
            });
        }
    }
}