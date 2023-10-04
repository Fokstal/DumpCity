using DumpCity.Data;
using DumpCity.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connectionStrToDb = builder.Configuration.GetConnectionString("FileConnection") ?? "";

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(ops =>
    ops.UseSqlServer(connectionStrToDb));

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(op => {
    op.IdleTimeout = TimeSpan.FromMinutes(10);
    op.Cookie.HttpOnly = true;
    op.Cookie.IsEssential = true;
});

// If your want to change an email adress -> then correct info in file | appsettings.json |
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders().AddDefaultUI()
    .AddEntityFrameworkStores<AppDbContext>();
    

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
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpt => {
    endpt.MapRazorPages();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Load}/{id?}");

app.Run();
