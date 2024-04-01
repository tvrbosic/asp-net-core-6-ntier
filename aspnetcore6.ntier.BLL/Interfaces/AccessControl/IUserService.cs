using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.BLL.DTOs.Shared;

namespace aspnetcore6.ntier.BLL.Interfaces.AccessControl
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsers();

        Task<PaginatedDataDTO<UserDTO>> GetPaginatedUsers(
            int PageNumber,
            int PageSize,
            string? searchInput,
            string[]? searchProperties,
            string orderByProperty = "Id",
            bool ascending = true);

        UserDTO GetUser(int id);

        Task<bool> AddUser(AddUserDTO role);

        Task<bool> UpdateUser(UpdateUserDTO role);

        Task<bool> DeleteUser(int id);
    }
}
