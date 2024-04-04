using aspnetcore6.ntier.DataAccess.Exceptions;
using aspnetcore6.ntier.DataAccess.Interfaces.Repositories.AccessControl;
using aspnetcore6.ntier.Models.AccessControl;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace aspnetcore6.ntier.DataAccess.Repositories.AccessControl
{
    public class UserRepository: Repository<ApplicationUser>, IUserRepository
    {

        public UserRepository(ApiDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<ApplicationUser>> GetAll()
        {
            IEnumerable<ApplicationUser> users = await _dbSet
                .Include(u => u.Department)
                .Include(u => u.RoleLinks)
                .ThenInclude(pl => pl.Role)
                .Where(u => u.Id != 1) // Filter out SUPERUSER
                .ToListAsync();

            if (users == null)
            {
                throw new EntityNotFoundException($"No users found.");
            }

            return users;
        }

        public override async Task<ApplicationUser> GetById(int id)
        {
            if (id == 1)
            {
                throw new SecurityException("Access to user with ID 1 is forbidden!");
            }

            ApplicationUser? user = await _dbSet
                .Include(u => u.Department)
                .Include(u => u.RoleLinks)
                .ThenInclude(pl => pl.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException($"Get operation failed for entitiy {typeof(ApplicationUser)} with id: {id}");
            }

            return user;
        }

        public async Task<ApplicationUser> GeByUsername(string username)
        {
            ApplicationUser? user = await _dbSet
                .Include(u => u.Department)
                .Include(u => u.RoleLinks)
                .ThenInclude(pl => pl.Role)
                .FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(username.ToLower()));

            if (user == null)
            {
                throw new EntityNotFoundException($"Get operation failed for entitiy {typeof(ApplicationUser)} with usename: {username}");
            }

            return user;
        }

        public override async Task Update(ApplicationUser user)
        {
            if (user.Id == 1)
            {
                throw new SecurityException("Update of user with ID 1 is forbidden!");
            }

            await base.Update(user);
        }

        public override async Task Delete(int id)
        {
            if (id == 1)
            {
                throw new SecurityException("Deletion of user with ID 1 is forbidden!");
            }

            await base.Delete(id);
        }
    }
}
