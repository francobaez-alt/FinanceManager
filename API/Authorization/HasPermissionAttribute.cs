using Microsoft.AspNetCore.Authorization;

namespace API.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
    {
        Policy = $"{PermissionPolicies.Prefix}{permission}";
    }
}
