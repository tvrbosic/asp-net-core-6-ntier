using Microsoft.AspNetCore.Authentication.Negotiate;
using aspnetcore6.ntier.API.Middleware;
using aspnetcore6.ntier.Services.Interfaces.AccessControl;
using aspnetcore6.ntier.Services.Interfaces.General;
using aspnetcore6.ntier.Services.Interfaces.Utilities;
using aspnetcore6.ntier.Services.Services.General;
using aspnetcore6.ntier.Services.Utilities;
using aspnetcore6.ntier.DataAccess.Interfaces.Repositories;
using aspnetcore6.ntier.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using aspnetcore6.ntier.Services.Services.AccessControl;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

#endregion

#region Swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Register the Swagger generator
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add a custom operation filter to inject the Windows authentication parameter to SwaggerUI
    c.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>();
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
builder.Services.AddScoped<IDataSeedService, DataSeedService>();
#endregion


var app = builder.Build();

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


// Custom operation filter to add Windows authentication header to Swagger UI
public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // If endpoint requires authentication
        if (context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AuthorizeAttribute)))
        {
            // Add Windows authentication header
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("Negotiate")
                }
            });
        }
    }
}