using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroStack.Core.Entitties;
using MicroStack.Core.Repositories;
using MicroStack.Core.Repositories.Base;
using MicroStack.Infrastructure.Data;
using MicroStack.Infrastructure.Repository;
using MicroStack.Infrastructure.Repository.Base;
using MicroStack.UI.Clients;
using MicroStack.UI.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<WebAppContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
//builder.Services.AddIdentity<AppUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<WebAppContext>();

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 4;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireDigit = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<WebAppContext>();

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.Cookie.Name = "My Cookie";
//        options.LoginPath = "Home/Login";
//        options.LogoutPath = "Home/Logout";
//        options.ExpireTimeSpan = TimeSpan.FromDays(3);
//        options.SlidingExpiration = false;
//    });
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Home/Login";
    options.LogoutPath = $"/Home/Logout";
});
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ProductClient>();
builder.Services.AddHttpClient<AuctionClient>();
builder.Services.AddHttpClient<BidClient>();
builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(20);
});

var app = builder.Build();

DbSeeder.CreateAndSeedDatabase(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


app.Run();


