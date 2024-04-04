using aspnetcore6.ntier.DataAccess.Exceptions;
using aspnetcore6.ntier.Models.AccessControl;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace aspnetcore6.ntier.DataAccess.Repositories.AccessControl
{
    public class UserRepository: Repository<ApplicationUser>
    {

        public UserRepository(ApiDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<ApplicationUser>> GetAll()
        {
            IEnumerable<ApplicationUser> roles = await _dbSet
                .Include(u => u.Department)
                .Include(u => u.RoleLinks)
                .ThenInclude(pl => pl.Role)
                .Where(u => u.Id != 1) // Filter out SUPERUSER
                .ToListAsync();

            if (roles == null)
            {
                throw new EntityNotFoundException($"No roles found.");
            }

            return roles;
        }

        public override async Task<ApplicationUser> GetById(int id)
        {
            if (id == 1)
            {
                throw new SecurityException("Access to user with ID 1 is forbidden!");
            }

            ApplicationUser? role = await _dbSet
                .Include(u => u.Department)
                .Include(u => u.RoleLinks)
                .ThenInclude(pl => pl.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (role == null)
            {
                throw new EntityNotFoundException($"Get operation failed for entitiy {typeof(ApplicationUser)} with id: {id}");
            }

            return role;
        }

        public override async Task Update(ApplicationUser entity)
        {
            if (entity.Id == 1)
            {
                throw new SecurityException("Update of user with ID 1 is forbidden!");
            }

            await base.Update(entity);
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
