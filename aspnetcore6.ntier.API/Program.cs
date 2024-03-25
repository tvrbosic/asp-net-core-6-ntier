using aspnetcore6.ntier.BLL.Services.General;
using aspnetcore6.ntier.BLL.Services.General.Interfaces;
using aspnetcore6.ntier.BLL.Utilities;
using aspnetcore6.ntier.BLL.Utilities.Interfaces;
using aspnetcore6.ntier.DAL.Repositories;
using aspnetcore6.ntier.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


#region Database context configuration
builder.Services.AddDbContext<ApiDbContext>(options => {
    if (builder.Environment.IsDevelopment())
    {
        options.UseSqlServer(configuration.GetConnectionString("ApiDb"))
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
    else
    {
        options.UseSqlServer(configuration.GetConnectionString("ApiDb"));
    }
});
#endregion

#region Serilog configuration
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.AddSerilog(logger);
#endregion

#region ASP.NET and third party services
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

#region Application services registration
// Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

// Utility
builder.Services.AddScoped<IDataSeed, DataSeed>();
#endregion


var app = builder.Build();

#region Seed data based on environment
using (var scope = app.Services.CreateScope())
{
    var dataSeed = scope.ServiceProvider.GetRequiredService<IDataSeed>();

    if (app.Environment.IsDevelopment())
    {
        await dataSeed.DevelopmentDataSeed();
    }
    else if (app.Environment.IsEnvironment("Test"))
    {
        await dataSeed.TestDataSeed();
    }
    else if (app.Environment.IsEnvironment("Uat"))
    {
        await dataSeed.UatDataSeed();
    }
    else if (app.Environment.IsProduction())
    {
        await dataSeed.ProductionDataSeed();
    }
}
#endregion

#region Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion