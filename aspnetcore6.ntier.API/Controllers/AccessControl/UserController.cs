using aspnetcore6.ntier.API.Requests;
using aspnetcore6.ntier.API.Responses;
using aspnetcore6.ntier.Services.DTO.AccessControl;
using aspnetcore6.ntier.Services.DTO.Shared;
using aspnetcore6.ntier.Services.Interfaces.AccessControl;
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
        public async Task<ActionResult<ApiDataResponse<IEnumerable<UserDTO>>>> GetUsers()
        {
            IEnumerable<UserDTO> users = await _userService.GetUsers();
            var response = new ApiDataResponse<IEnumerable<UserDTO>>(users, "Users retrieved succcessfully.");
            return Ok(response);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<ApiPagnatedResponse<UserDTO>>> GetPaginatedUsers([FromQuery] PaginateQueryParameters qp)
        {
            PaginatedDataDTO<UserDTO> pu = await _userService.GetPaginatedUsers(
                qp.PageNumber,
                qp.PageSize,
                qp.searchText,
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
        public async Task<ActionResult<ApiDataResponse<UserDTO>>> GetUser(int id)
        {
            UserDTO user = await _userService.GetUser(id);
            var response = new ApiDataResponse<UserDTO>(user, "User retrieved succcessfully.");
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiBaseResponse>> PostUser(AddUserDTO userDTO)
        {
            await _userService.AddUser(userDTO);
            var response = new ApiBaseResponse("User created succcessfully.");
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ApiBaseResponse>> PutUser(UpdateUserDTO userDTO)
        {
            await _userService.UpdateUser(userDTO);
            var response = new ApiBaseResponse("User updated succcessfully.");
            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ApiBaseResponse>> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            var response = new ApiBaseResponse("User deleted succcessfully.");
            return Ok(response);
        }
    }
}
