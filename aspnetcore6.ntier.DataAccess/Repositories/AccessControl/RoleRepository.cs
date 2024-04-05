using aspnetcore6.ntier.DataAccess.Exceptions;
using aspnetcore6.ntier.DataAccess.Interfaces.Repositories;
using aspnetcore6.ntier.Models.AccessControl;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore6.ntier.DataAccess.Repositories.AccessControl
{
    public class RoleRepository : Repository<Role>
    {

        public RoleRepository(ApiDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Role>> GetAll()
        {
            IEnumerable<Role> roles = await _dbSet
                .Include(r => r.Department)
                .Include(r => r.PermissionLinks)
                    .ThenInclude(pl => pl.Permission)
                .ToListAsync();

            if (roles == null)
            {
                throw new EntityNotFoundException("No roles found.");
            }

            return roles;
        }

        public override async Task<Role> GetById(int id)
        {
            Role? role = await _dbSet
                .Include(r => r.Department)
                .Include(r => r.PermissionLinks)
                .ThenInclude(pl => pl.Permission)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
            {
                throw new EntityNotFoundException($"Get operation failed for entitiy {typeof(Role)} with id: {id}");
            }

            return role;
        }
    }
}
