﻿using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.BLL.DTOs.Shared;
using aspnetcore6.ntier.BLL.Interfaces.AccessControl;
using aspnetcore6.ntier.DAL.Exceptions;
using aspnetcore6.ntier.DAL.Interfaces.Repositories;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Models.Shared;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace aspnetcore6.ntier.BLL.Services.AccessControl
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
            IEnumerable<ApplicationUser> users = await _unitOfWork.Users
                .Queryable()
                .Include(u => u.Department)
                .Include(u => u.RoleLinks)
                .ThenInclude(pl => pl.Role)
                .ToListAsync();
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
            ApplicationUser? user = await _unitOfWork.Users
                .Queryable()
                .Include(u => u.Department)
                .Include(u => u.RoleLinks)
                .ThenInclude(pl => pl.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException($"Get operation failed for entitiy {typeof(ApplicationUser)} with id: {id}");
            }
            else
            {
                UserDTO userDTO = _mapper.Map<UserDTO>(user);
                return userDTO;
            }
        }

        public async Task<UserDTO> GetUserByUsername(string userName)
        {
            IEnumerable<ApplicationUser> users = await _unitOfWork.Users.Find(u => u.UserName.Equals(userName));
            var user = users.FirstOrDefault();

            if (user == null)
            {
                throw new EntityNotFoundException($"Get operation failed for entitiy {typeof(ApplicationUser)} with username: {userName}");
            }
            else
            {
                UserDTO userDTO = _mapper.Map<UserDTO>(user);
                return userDTO;
            }
        }

        public async Task<bool> AddUser(AddUserDTO userDTO)
        {
            ApplicationUser addUser = _mapper.Map<ApplicationUser>(userDTO);

            // Add roles to user from provided roleIds
            foreach (int roleId in userDTO.RoleIds)
            {
                Role? roleToAdd = await _unitOfWork.Roles.GetById(roleId);
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
            ApplicationUser? updateUser = await _unitOfWork.Users
                .Queryable()
                .Include(r => r.Department)
                .Include(r => r.RoleLinks)
                .ThenInclude(rl => rl.Role)
                .FirstOrDefaultAsync(r => r.Id == userDTO.Id);

            if (updateUser == null)
            {
                throw new EntityNotFoundException($"Update operation failed for entitiy {typeof(ApplicationUser)} with id: {userDTO.Id}");
            }

            _mapper.Map(userDTO, updateUser);

            // Clear previously given permissions
            updateUser.RoleLinks.Clear();

            foreach (int roleId in userDTO.RoleIds)
            {
                Role? roleToAdd = await _unitOfWork.Roles.GetById(roleId);
                if (roleToAdd != null)
                {
                    updateUser.RoleLinks.Add(new RoleUserLink
                    {
                        User = updateUser,
                        Role = roleToAdd
                    });
                }
                
            }
            await _unitOfWork.Users.Update(updateUser);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> DeleteUser(int id)
        {
            await _unitOfWork.Users.Delete(id);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
