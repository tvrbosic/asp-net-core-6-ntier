using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Repositories.AccessControl.Interfaces;

namespace aspnetcore6.ntier.DAL.Repositories.AccessControl
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly ApiDbContext _context;

        public RoleRepository(ApiDbContext context): base(context)
        {
            _context = context;
        }

        public async Task AddWithRoles(Role role, List<int> permissionIds)
        {
            foreach (int permissionId in permissionIds)
            {
                Permission permissionToAdd = _context.Permissions.Single(p => p.Id == permissionId);
                role.Permissions.Add(permissionToAdd);
            }

            role.DateCreated = DateTime.UtcNow;
            await _context.Roles.AddAsync(role);
        }
    }
}
