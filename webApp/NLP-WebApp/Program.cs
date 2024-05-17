using Microsoft.AspNetCore.Authentication.Cookies;
using NLP_WebApp.Abstract;
using NLP_WebApp.Models.Entity;
using NLP_WebApp.Models.Data;
using NLP_WebApp.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Context>();
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<Context>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Home/LogAndReg";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
});

builder.Services.AddScoped<IInterest, efInterest>();
builder.Services.AddHttpClient();

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
    pattern: "{controller=Home}/{action=LogAndReg}/{id?}");

app.Run();
