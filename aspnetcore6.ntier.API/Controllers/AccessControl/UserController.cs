using aspnetcore6.ntier.API.Requests;
using aspnetcore6.ntier.API.Responses;
using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.BLL.DTOs.Shared;
using aspnetcore6.ntier.BLL.Interfaces.AccessControl;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore6.ntier.API.Controllers.AccessControl
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            IEnumerable<UserDTO> users = await _userService.GetUsers();
            var response = new ApiDataResponse<IEnumerable<UserDTO>>(users, "Users retrieved succcessfully.");
            return Ok(response);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetPaginatedUsers([FromQuery] PaginateQueryParameters qp)
        {
            PaginatedDataDTO<UserDTO> pu = await _userService.GetPaginatedUsers(
                qp.PageNumber,
                qp.PageSize,
                qp.searchInput,
                qp.searchProperties,
                qp.orderByProperty,
                qp.ascending);

            var response = new ApiPagnatedResponse<UserDTO>(
                pu.Data,
                pu.PageNumber,
                pu.TotalPages,
                pu.PageSize,
                pu.TotalCount,
                pu.HasPrevious,
                pu.HasNext,
                "Users retrieved succcessfully.");

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDTO> GetUser(int id)
        {
            UserDTO user = _userService.GetUser(id);
            var response = new ApiDataResponse<UserDTO>(user, "User retrieved succcessfully.");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> PostUser(AddUserDTO userDTO)
        {
            await _userService.AddUser(userDTO);
            var response = new ApiBaseResponse("User created succcessfully.");
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> PutUser(UpdateUserDTO userDTO)
        {
            await _userService.UpdateUser(userDTO);
            var response = new ApiBaseResponse("User updated succcessfully.");
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            var response = new ApiBaseResponse("User deleted succcessfully.");
            return Ok(response);
        }
    }
}
