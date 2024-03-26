using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Repositories.AccessControl.Interfaces;
using System.Linq;

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

        public async  Task Update(Role role, List<int> permissionIds)
        {
            // Remove permissions that are not in the provided list
            //List<Permission> permissionsToRemove = role.Permissions.Where(rp => !permissionIds.Contains(rp.Id)).ToList();
            //foreach (var permission in role.Permissions)
            //{
            //    role.Permissions.Remove(permission);
            //}
            
            
            role.Permissions.Clear();
            //foreach (Permission permission in role.Permissions.ToList())
            //{
            //    role.Permissions.Remove(permission);
            //}
            //await _context.SaveChangesAsync();

            // Add missing permissions
            //List<Permission> permissionsToAdd = _context.Permissions.Where(p => permissionIds.Contains(p.Id)).ToList();
            //foreach (var permission in permissionsToAdd)
            //{
            //    if (!role.Permissions.Any(rp => rp.Id == permission.Id))
            //    {
            //        role.Permissions.Add(permission);
            //    }
            //}

            foreach (int permissionId in permissionIds)
            {
                Permission permissionToAdd = _context.Permissions.Single(p => p.Id == permissionId);
                role.Permissions.Add(permissionToAdd);
            }

            _context.Roles.Update(role);
        }

        public async Task Delete(int id)
        {
            // TODO
        }
    }
}
