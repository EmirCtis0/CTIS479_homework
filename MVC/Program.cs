using APP.Domain;
using APP.Models;
using APP.Services;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the IoC container.
var connectionString = builder.Configuration.GetConnectionString(nameof(Db));
builder.Services.AddDbContext<DbContext, Db>(options => options.UseSqlite(connectionString));

//builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();


builder.Services.AddControllersWithViews();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Db>();

    // Eðer veritabaný yoksa oluþtur
    db.Database.EnsureCreated();

    // Buraya istediðin roller:
    var requiredRoles = new[] { "Admin", "User", "Main Admin" };

    var existingNames = db.Roles
        .Select(r => r.Name)
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    var toAdd = requiredRoles
        .Where(name => !existingNames.Contains(name))
        .Select(name => new Role { Name = name })
        .ToList();

    if (toAdd.Count > 0)
    {
        db.Roles.AddRange(toAdd);
        db.SaveChanges();
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
