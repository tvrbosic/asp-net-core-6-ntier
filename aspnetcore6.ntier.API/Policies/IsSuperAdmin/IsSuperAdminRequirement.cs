using Microsoft.AspNetCore.Authorization;

namespace aspnetcore6.ntier.API.Policies.IsSuperAdmin
{
    public class IsSuperAdminRequirement : IAuthorizationRequirement
    {
        public IsSuperAdminRequirement() { }
    }
}
