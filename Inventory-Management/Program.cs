using Inventory_Management.Data;
using Inventory_Management.Repositories;
using Inventory_Management.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Service Registrations
// Add services to the container.
builder.Services.AddControllersWithViews();
IConfiguration configuration = builder.Configuration;

builder.Services.AddScoped<IUserService, UserRepository>();
builder.Services.AddScoped<ICategoryService, CategoryRepository>();
builder.Services.AddScoped<IProductService, ProductRepository>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryRepository>();
builder.Services.AddScoped<IAuditLogService, AuditLogRepository>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Inventory Management API",
        Version = "v1",
        Description = "API for managing inventory, products, and categories.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Your Company",
            Email = "your-email@company.com",
            Url = new Uri("https://www.yourcompany.com")
        }
    });
});
#endregion

#region DB Connection
// Local
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
    });
}, ServiceLifetime.Scoped);
#endregion

#region Build Application
var app = builder.Build();
#endregion

#region HTTP Request Pipeline
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseSwagger(); // Enable Swagger
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory Management API v1");
        c.RoutePrefix = string.Empty; // Swagger UI will be available at the root
    });
}
#endregion

#region Middleware Configuration
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
#endregion

#region Route Configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Products}");
#endregion

#region Application Execution
app.Run();
#endregion
