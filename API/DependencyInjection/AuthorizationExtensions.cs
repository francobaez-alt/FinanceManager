using API.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace API.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddCustomAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddSingleton<IAuthorizationHandler,
            PermissionAuthorizationHandler>();

        services.AddSingleton<IAuthorizationPolicyProvider,
            PermissionPolicyProvider>();

        return services;
    }
}
