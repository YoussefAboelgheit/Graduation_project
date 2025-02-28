using Microsoft.EntityFrameworkCore;
using Test1._1.Models.Entity;

var builder = WebApplication.CreateBuilder(args);

// 🔹 تسجيل `AppDBContext` مع استخدام `Connection String` من `appsettings.json`
builder.Services.AddDbContext<AppDBContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
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
