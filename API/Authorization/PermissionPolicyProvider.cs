using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace API.Authorization;

public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    {
    }

    public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(PermissionPolicies.Prefix))
        {
            var permission = policyName[PermissionPolicies.Prefix.Length..];

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(permission))
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return base.GetPolicyAsync(policyName);
    }
}