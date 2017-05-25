using Guilded.Security.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Guilded.Security.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddRequirementHandlers(this IServiceCollection services)
        {
            services.AddTransient<IAuthorizationHandler, RoleClaimAuthorizationHandler>();
            services.AddTransient<IAuthorizationHandler, EnabledUserHandler>();
        }
    }
}
