using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Inventory_Management.Data;
using Microsoft.OpenApi.Models;
using Inventory_Management.Handler;
using Inventory_Management.Services;
using Inventory_Management.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
IConfiguration configuration = builder.Configuration;


builder.Services.AddScoped<IProductService, ProductRepository>();
builder.Services.AddScoped<ICategoryService, CategoryRepository>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Central Server API", Version = "v1" });
});


#region DB Connection
//Local
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    

        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
        });
    
}, ServiceLifetime.Scoped);

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inovace Central Server");
    });
}


app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Products}");

app.Run();
