using aspnetcore6.ntier.Services.DTO.AccessControl;
using aspnetcore6.ntier.Services.DTO.Shared;
using aspnetcore6.ntier.Services.Interfaces.AccessControl;
using aspnetcore6.ntier.DataAccess.Interfaces.Repositories;
using aspnetcore6.ntier.Models.AccessControl;
using aspnetcore6.ntier.Models.Shared;
using AutoMapper;
using System.Linq.Expressions;

namespace aspnetcore6.ntier.Services.Services.AccessControl
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            IEnumerable<ApplicationUser> users = await _unitOfWork.Users.GetAll();
            IEnumerable<UserDTO> userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
            return userDTOs;
        }

        public async Task<PaginatedDataDTO<UserDTO>> GetPaginatedUsers(
            int PageNumber,
            int PageSize,
            string? searchText,
            string orderByProperty = "Id",
            bool ascending = true)
        {
            Expression<Func<ApplicationUser, bool>>? searchTextPredicate = null;
            if (!string.IsNullOrEmpty(searchText))
            {
                searchTextPredicate = p => 
                    p.UserName.ToLower().Contains(searchText.ToLower()) ||
                    p.FirstName.ToLower().Contains(searchText.ToLower()) ||
                    p.LastName.ToLower().Contains(searchText.ToLower()) ||
                    p.Email.ToLower().Contains(searchText.ToLower());
            }

            PaginatedData<ApplicationUser> paginatedUsers = await _unitOfWork.Users.GetAllPaginated(
                PageNumber,
                PageSize,
                searchTextPredicate,
                orderByProperty,
                ascending);
            PaginatedDataDTO<UserDTO> paginatedUserDTOs = _mapper.Map<PaginatedDataDTO<UserDTO>>(paginatedUsers);

            return paginatedUserDTOs;
        }

        public async Task<UserDTO> GetUser(int id)
        {
            ApplicationUser user = await _unitOfWork.Users.GetById(id);           
            UserDTO userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task<bool> AddUser(AddUserDTO userDTO)
        {
            ApplicationUser addUser = _mapper.Map<ApplicationUser>(userDTO);

            // Add roles to user from provided roleIds
            foreach (int roleId in userDTO.RoleIds)
            {
                Role roleToAdd = await _unitOfWork.Roles.GetById(roleId);
                if (roleToAdd != null)
                {
                    addUser.RoleLinks.Add(new RoleUserLink
                    {
                        User = addUser,
                        Role = roleToAdd
                    });
                }
            }

            await _unitOfWork.Users.Add(addUser);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdateUser(UpdateUserDTO userDTO)
        {
            ApplicationUser updateUser = await _unitOfWork.Users.GetById(userDTO.Id);

            _mapper.Map(userDTO, updateUser);

            // Clear previously given permissions
            updateUser.RoleLinks.Clear();

            foreach (int roleId in userDTO.RoleIds)
            {
                Role roleToAdd = await _unitOfWork.Roles.GetById(roleId);
                updateUser.RoleLinks.Add(new RoleUserLink
                {
                    User = updateUser,
                    Role = roleToAdd
                });
            }
            await _unitOfWork.Users.Update(updateUser);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> DeleteUser(int id)
        {
            ApplicationUser updateUser = await _unitOfWork.Users.GetById(id);
            
            updateUser.RoleLinks.Clear();
            
            await _unitOfWork.Users.Delete(id);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
