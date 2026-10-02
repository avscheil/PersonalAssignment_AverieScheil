using Microsoft.EntityFrameworkCore;
using PersonalAssignment_AverieScheil.Models;
using Microsoft.AspNetCore.Identity;
using PersonalAssignment_AverieScheil.Data;
//using PersonalAssignment_AverieScheil.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TheContextConnection") ?? throw new InvalidOperationException("Connection string 'TheContextConnection' not found."); ;

builder.Services.AddDbContext<TheContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TheContextConnection"));
});

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<TheContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Razor pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
