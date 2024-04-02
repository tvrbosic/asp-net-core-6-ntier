#nullable disable
using Microsoft.EntityFrameworkCore;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Models.General;
using aspnetcore6.ntier.DAL.Models.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using aspnetcore6.ntier.DAL.Interfaces.Abstract;
using Microsoft.AspNetCore.Http;

public class ApiDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiDbContext(DbContextOptions<ApiDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    #region General entity registration
    public DbSet<Department> Departments { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    #endregion

    #region Access control entity registration
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<PermissionRoleLink> PermissionRoleLinks { get; set; }
    public DbSet<RoleUserLink> RoleUserLinks { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        #region Base entity configuration
        new BaseEntityConfiguration<Department>().Configure(modelBuilder.Entity<Department>());
        new BaseEntityConfiguration<Permission>().Configure(modelBuilder.Entity<Permission>());
        new BaseEntityConfiguration<Role>().Configure(modelBuilder.Entity<Role>());
        new BaseEntityConfiguration<ApplicationUser>().Configure(modelBuilder.Entity<ApplicationUser>());
        new BaseEntityConfiguration<PermissionRoleLink>().Configure(modelBuilder.Entity<PermissionRoleLink>());
        new BaseEntityConfiguration<RoleUserLink>().Configure(modelBuilder.Entity<RoleUserLink>());
        #endregion

        #region General entitiy configuration
        modelBuilder.Entity<Department>(entity =>
        {
            entity
                .HasMany(d => d.Users)
                .WithOne(u => u.Department)
                .HasForeignKey(u => u.DepartmentId);
            entity
                .HasMany(d => d.Roles)
                .WithOne(r => r.Department)
                .HasForeignKey(r => r.DepartmentId);
            entity
                .HasMany(d => d.Permissions)
                .WithOne(u => u.Department)
                .HasForeignKey(u => u.DepartmentId);
        });
        #endregion

        #region Access control entity configuration
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity
                .HasOne(r => r.Department)
                .WithMany(d => d.Roles)
                .HasForeignKey(r => r.DepartmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity
                .HasOne(p => p.Department)
                .WithMany(d => d.Permissions)
                .HasForeignKey(p => p.DepartmentId)
                .IsRequired();
        });

        modelBuilder.Entity<PermissionRoleLink>(entity =>
        {
            entity.HasKey(pl => pl.Id);
        });

        modelBuilder.Entity<RoleUserLink>(entity =>
        {
            entity.HasKey(ru => ru.Id);
        });

        #endregion

        #region Query filters
        // =======================================| SOFT DELETE |======================================= //
        modelBuilder.Entity<Department>().HasQueryFilter(d => !d.IsDeleted);
        modelBuilder.Entity<Permission>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<ApplicationUser>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<PermissionRoleLink>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<RoleUserLink>().HasQueryFilter(p => !p.IsDeleted);
        #endregion
    }

    // =======================================| IMPORTANT |======================================= //
    #region Global context configuration, method overrides, utility methods
    public override int SaveChanges()
    {
        ProcessChangetrackerEntries();
        ProcessAuditLog();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ProcessChangetrackerEntries();
        ProcessAuditLog(); 
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ProcessChangetrackerEntries()
    {
        // Get entities from database context which are extended from BaseEntitiy class prior to executing database query
        var modifiedEntries = this.ChangeTracker.Entries()
            .Where(e => (e.Entity is BaseEntity) || (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));

        var authenticatedUser = GetAuthenticatedUser();

        // Update BaseEntity properties according to entity state 
        foreach (var entry in modifiedEntries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property("CreatedById").CurrentValue = authenticatedUser.Id;
                    entry.Property("DateCreated").CurrentValue = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Property("UpdatedById").CurrentValue = authenticatedUser.Id;
                    entry.Property("DateUpdated").CurrentValue = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    entry.Property("DeletedById").CurrentValue = authenticatedUser.Id;
                    entry.Property("DateDeleted").CurrentValue = DateTime.UtcNow;
                    entry.Property("IsDeleted").CurrentValue = true;
                    entry.State = EntityState.Modified;
                    // Cascade soft delete navigation properties
                    ProcessCascadeSoftDelete(entry, authenticatedUser);
                    break;
            }
        }
    }

    private void ProcessCascadeSoftDelete(EntityEntry entry, ApplicationUser authenticatedUser)
    {
        foreach (var navigationEntry in entry.Navigations)
        {
            if (navigationEntry is CollectionEntry collectionEntry)
            {
                if(!navigationEntry.IsLoaded) navigationEntry.Load();
                foreach (var dependentEntry in collectionEntry.CurrentValue)
                {
                    SoftDeleteIfNotProtected(dependentEntry, authenticatedUser);
                }
            }
            else
            {
                var dependentEntry = navigationEntry.CurrentValue;
                if (dependentEntry != null)
                {
                    SoftDeleteIfNotProtected(dependentEntry, authenticatedUser);
                }
            }
        }
    }

    private void ProcessAuditLog()
    {
        var authenticatedUser = GetAuthenticatedUser();
        var auditLogEntries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
            .Select(e =>
            {
                var entryData = JsonSerializer.Serialize(e.Properties.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue));
                var newAuditLog = new AuditLog
                {
                    AuditKey = (Guid)e.Properties.Where(p => p.Metadata.Name.ToUpper() == "AUDITKEY").Select(p => p.CurrentValue).FirstOrDefault(),
                    UserId = authenticatedUser.Id,
                    Username = authenticatedUser.UserName,
                    Operation = (Boolean) e.Properties
                        .Where(p => p.Metadata.Name.ToUpper() == "ISDELETED")
                        .Select(p => p.CurrentValue)
                        .FirstOrDefault() == true ? EntityState.Deleted.ToString() : e.State.ToString(),
                    Timestamp = DateTime.UtcNow,
                    EntityName = e.Entity.GetType().Name,
                    EntityData = entryData,
                };
                return newAuditLog;
            }).ToList();

        AuditLogs.AddRange(auditLogEntries);
    }

    private void SoftDeleteIfNotProtected(object entity, ApplicationUser authenticatedUser)
    {
        var isSoftDeleteProtected = entity is ISoftDeleteProtectedEntity protectedEntity && protectedEntity.IsSoftDeleteProtected;
        if (!isSoftDeleteProtected)
        {
            Entry(entity).Property("DeletedById").CurrentValue = authenticatedUser.Id;
            Entry(entity).Property("DateDeleted").CurrentValue = DateTime.UtcNow;
            Entry(entity).Property("IsDeleted").CurrentValue = true;
            Entry(entity).State = EntityState.Modified;
        }
    }

    private ApplicationUser GetAuthenticatedUser()
    {
        var  authenticatedUserName = _httpContextAccessor.HttpContext.User.Identity.Name;
        
        if (authenticatedUserName != null)
        {
            // AsEnumberable is required because of previously configured query filter on Users table (in this file)
            IEnumerable<ApplicationUser> findUserResult = this.Users.AsEnumerable().Where(u => u.UserName.ToLower().Equals(authenticatedUserName.ToLower()));

            if (findUserResult.Any())
            {
                return findUserResult.FirstOrDefault();
            }
            else
            {
                throw new UnauthorizedAccessException("There is no user with provided username in application database!");
            }
        }
        else
        {
            throw new UnauthorizedAccessException("Database context is unable to read authenticated user name from HTTP context!");
        }
    }
    #endregion
}


public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .HasOne(b => b.CreatedBy)
            .WithMany()
            .HasForeignKey(b => b.CreatedById);
        builder
            .HasOne(b => b.UpdatedBy)
            .WithMany()
            .HasForeignKey(b => b.UpdatedById);
        builder
            .HasOne(b => b.DeletedBy)
            .WithMany()
            .HasForeignKey(b => b.DeletedById);
    }
}