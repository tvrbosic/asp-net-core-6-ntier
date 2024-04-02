using Microsoft.AspNetCore.Authentication.Negotiate;
using aspnetcore6.ntier.API.Middleware;
using aspnetcore6.ntier.BLL.Interfaces.AccessControl;
using aspnetcore6.ntier.BLL.Interfaces.General;
using aspnetcore6.ntier.BLL.Interfaces.Utilities;
using aspnetcore6.ntier.BLL.Services.General;
using aspnetcore6.ntier.BLL.Utilities;
using aspnetcore6.ntier.DAL.Interfaces.Repositories;
using aspnetcore6.ntier.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using aspnetcore6.ntier.BLL.Services.AccessControl;

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
builder.Services.AddHttpContextAccessor();

// =======================================| SWAGGER |======================================= //
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// =======================================| AUTHENTICATION |======================================= //
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

#endregion

#region Application services registration
// =======================================| REPOSITORIES |======================================= //
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// =======================================| SERVICES |======================================= //
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();

// =======================================| UTILITY |======================================= //
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

#region Configure the HTTP request pipeline
// REMINDER: Keep in mind that middleware invoking order is important!

// Global exception handler (should be at the top)
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion