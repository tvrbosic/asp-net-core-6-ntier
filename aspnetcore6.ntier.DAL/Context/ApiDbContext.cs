using Microsoft.EntityFrameworkCore;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Models.General;
using aspnetcore6.ntier.DAL.Models.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }
    #region General entity registration
    public DbSet<Department> Departments { get; set; }
    #endregion

    #region Access control entity registration
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    #endregion


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        #region Base entity configuration
        new BaseEntityConfiguration<Department>().Configure(modelBuilder.Entity<Department>());
        new BaseEntityConfiguration<Permission>().Configure(modelBuilder.Entity<Permission>());
        new BaseEntityConfiguration<Role>().Configure(modelBuilder.Entity<Role>());
        new BaseEntityConfiguration<User>().Configure(modelBuilder.Entity<User>());
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
        modelBuilder.Entity<User>(entity =>
        {
            entity
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .IsRequired();
            entity
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(
                    "RoleUser",
                    l => l.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
                    r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(User.Id)),
                    j => j.HasKey("RoleId", "UserId"));
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity
                .HasOne(r => r.Department)
                .WithMany(d => d.Roles)
                .HasForeignKey(r => r.DepartmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            entity
                .HasMany(r => r.Users)
                .WithMany(u => u.Roles)
                .UsingEntity(
                    "RoleUser",
                    l => l.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
                    r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(User.Id)),
                    j => j.HasKey("RoleId", "UserId"));
            entity
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity(
                    "PermissionRole",
                    l => l.HasOne(typeof(Permission)).WithMany().HasForeignKey("PermissionId").HasPrincipalKey(nameof(Permission.Id)),
                    r => r.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
                    j => j.HasKey("PermissionId", "RoleId"));
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity
                .HasOne(p => p.Department)
                .WithMany(d => d.Permissions)
                .HasForeignKey(p => p.DepartmentId)
                .IsRequired();
            entity
                .HasMany(p => p.Roles)
                .WithMany(r => r.Permissions)
                .UsingEntity(
                    "PermissionRole",
                    l => l.HasOne(typeof(Permission)).WithMany().HasForeignKey("PermissionId").HasPrincipalKey(nameof(Permission.Id)),
                    r => r.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
                    j => j.HasKey("PermissionId", "RoleId"));
        });
        #endregion

        #region Soft delete entity filter registration
        modelBuilder.Entity<Department>().HasQueryFilter(d => !d.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<Permission>().HasQueryFilter(p => !p.IsDeleted);
        #endregion

        
    }
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