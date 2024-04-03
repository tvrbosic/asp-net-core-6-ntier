using aspnetcore6.ntier.DataAccess.Exceptions;
using aspnetcore6.ntier.Models.AccessControl;
using Microsoft.EntityFrameworkCore;

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
                .ToListAsync();

            if (roles == null)
            {
                throw new EntityNotFoundException($"No roles found.");
            }

            return roles;
        }

        public override async Task<ApplicationUser> GetById(int id)
        {
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
    }
}
