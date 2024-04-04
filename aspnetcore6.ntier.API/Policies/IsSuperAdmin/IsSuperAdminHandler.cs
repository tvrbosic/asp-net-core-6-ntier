using aspnetcore6.ntier.Services.Constants;
using aspnetcore6.ntier.Services.Interfaces.AccessControl;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace aspnetcore6.ntier.API.Policies.IsSuperAdmin
{
    public class IsSuperAdminHandler : AuthorizationHandler<IsSuperAdminRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly List<string> _requiredRoleNames = new List<string>();

        public IsSuperAdminHandler(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;

            // Initializing requiredRoleNames (user will be checked if it has this roles)
            _requiredRoleNames = new List<string>()
            {
                UserRoleConstants.SuperAdministrator
            };
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsSuperAdminRequirement requirement)
        {
            var user = _httpContextAccessor.HttpContext.User;

            // Check if the user identity is fully formed. If it is not fully formed we still do not fail the requirement but also we do not succeed it yet
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                // Getting user's name claim
                var userNameClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (userNameClaim != null && !string.IsNullOrWhiteSpace(userNameClaim.Value))
                {
                    // Retrieving user details from the user service
                    var appUser = await _userService.GetUserByUsername(userNameClaim.Value); 

                    if (appUser != null)
                    {
                        // Get user's role names
                        IEnumerable<string> userRoleNames = appUser.Roles.Select(r => r.Name);
                        
                        // Checking if user has required role
                        var isAuthorized = _requiredRoleNames.Any(r => userRoleNames.Contains(r));
                        if (isAuthorized)
                        {
                            // Succeed the authorization requirement
                            context.Succeed(requirement); 
                            return;
                        }
                    }

                    // Failing the authorization if user is not found or doesn't have the required role
                    context.Fail(); 
                    return;
                }

                // Failing the authorization if user's name claim is missing or empty
                context.Fail(); 
            }

            await Task.CompletedTask; 
        }
    }
}