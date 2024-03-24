using Microsoft.EntityFrameworkCore;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Models.General;
using aspnetcore6.ntier.DAL.Models.Abstract;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }
    #region General entity registration
    public DbSet<Department> Departments { get; set; }
    #endregion

    #region Access control entity registration
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    #endregion


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Base entity configuration
        modelBuilder.Entity<Department>(entity =>
        {
            entity
                .HasOne<User>(b => b.CreatedBy)
                .WithMany()
                .HasForeignKey(b => b.CreatedById);

            entity
                .HasOne<User>(b => b.UpdatedBy)
                .WithMany()
                .HasForeignKey(b => b.UpdatedById);

            entity
                .HasOne<User>(b => b.DeletedBy)
                .WithMany()
                .HasForeignKey(b => b.DeletedById);
        });
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
                .UsingEntity(j => j.ToTable("UserRole"));
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity
                .HasOne(r => r.Department)
                .WithMany(d => d.Roles)
                .HasForeignKey(r => r.DepartmentId)
                .IsRequired();
            entity
                .HasMany(r => r.Users)
                .WithMany(u => u.Roles)
                .UsingEntity(j => j.ToTable("UserRole"));
            entity
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity(j => j.ToTable("RolePermission"));
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
                .UsingEntity(j => j.ToTable("RolePermission"));
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