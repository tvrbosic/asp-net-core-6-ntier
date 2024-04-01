using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.BLL.DTOs.Shared;
using aspnetcore6.ntier.DAL.Models.AccessControl;

namespace aspnetcore6.ntier.BLL.Interfaces.AccessControl
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsers();

        PaginatedDataDTO<UserDTO> GetPaginatedUsers(
            int PageNumber,
            int PageSize,
            string? searchText,
            string orderByProperty = "Id",
            bool ascending = true);

        UserDTO GetUser(int id);

        Task<bool> AddUser(AddUserDTO role);

        Task<bool> UpdateUser(UpdateUserDTO role);

        Task<bool> DeleteUser(int id);
    }
}
