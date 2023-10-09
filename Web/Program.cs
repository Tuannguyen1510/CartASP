using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("BookStoreString");
builder.Services.AddDbContext<DevXuongMocContext>(options => options.UseSqlServer(connectionString));

// Cấu hình sử dụng session
builder.Services.AddDistributedMemoryCache();


// Đk dịch vụ cho HttpContextAccessor 
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".Dev.Session";
});



var app = builder.Build();

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

// SD session ở trên 
app.UseSession();




app.MapControllerRoute(
         name: "areas",
         pattern: "{area:exists}/{controller=Categories}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
