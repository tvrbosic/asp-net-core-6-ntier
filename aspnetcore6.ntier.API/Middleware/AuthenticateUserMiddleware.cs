using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.BLL.Interfaces.AccessControl;

namespace aspnetcore6.ntier.API.Middleware
{
    public class AuthenticateUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IUserService _userService;

        public AuthenticateUserMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IUserService userService)
        {
            _next = next;
            _logger = logger;
            _userService = userService;
        }

        public async Task Invoke(HttpContext context)
        {
            var userName = context.User.Identity.Name;
            if (userName != null)
            {
                // Retrieve user ID from wherever it's stored
                UserDTO authenticatedUser = await _userService.GetUserByUsername(userName);

                // Store user ID in HttpContext.Items
                context.Items["AuthenticatedUserId"] = authenticatedUser.Id;
            }
            // TODO: Throw error

            await _next(context);
        }

        private int GetUserIdFromWherever()
        {
            // Logic to retrieve user ID from wherever it's stored
            return 2; // Replace with actual implementation
        }
    }
}
