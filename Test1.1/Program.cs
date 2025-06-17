using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Stripe;
using Test1._1.Models.Entity;

var builder = WebApplication.CreateBuilder(args);

// 🔹 تسجيل `AppDBContext` مع استخدام `Connection String` من `appsettings.json`
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services - Updated to match your DbContext configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<AppDBContext>()
.AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(14); // ← مدة بقاء الكوكي لو Persistent
    options.SlidingExpiration = true;               // ← يعيد تجديد الكوكي إذا المستخدم نشط
});

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

app.UseAuthentication();
app.UseAuthorization();

// Configure Stripe
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<String>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();