using Microsoft.EntityFrameworkCore;
using aspnetcore6.ntier.DAL.Models.AccessControl;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    public DbSet<Department> Departments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>().HasQueryFilter(d => !d.IsDeleted);
    }
}