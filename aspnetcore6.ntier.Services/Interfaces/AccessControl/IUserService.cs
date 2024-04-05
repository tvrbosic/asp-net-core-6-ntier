using aspnetcore6.ntier.Services.DTO.AccessControl;
using aspnetcore6.ntier.Services.DTO.Shared;
using aspnetcore6.ntier.Models.AccessControl;

namespace aspnetcore6.ntier.Services.Interfaces.AccessControl
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsers();

        Task<PaginatedDataDTO<UserDTO>> GetPaginatedUsers(
            int PageNumber,
            int PageSize,
            string? searchText,
            string orderByProperty = "Id",
            bool ascending = true);

        Task<UserDTO> GetUser(int id);

        Task<UserDTO> GetUserByUsername(string username);

        Task<bool> AddUser(AddUserDTO role);

        Task<bool> UpdateUser(UpdateUserDTO role);

        Task<bool> DeleteUser(int id);
    }
}
