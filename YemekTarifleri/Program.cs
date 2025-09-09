using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using YemekTarifleri.Entity;
using YemekTarifleri.Data.Concrete.EfCore;
using YemekTarifleri.Data.Abstract;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using YemekTarifleri.Authorizon;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFoodRepository, EfFoodRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();
builder.Services.AddScoped<IIngredientRepository, EfIngredientRepository>();
builder.Services.AddScoped<IImageRepository, EfImageRepository>();
builder.Services.AddScoped<IStepRepository, EfStepRepository>();
builder.Services.AddScoped<IFtypeRepository, EfFtypeRepository>();
builder.Services.AddScoped<ILikeRepository, EfLikeRepository>();
builder.Services.AddScoped<IViewRepository, EfViewRepository>();
builder.Services.AddScoped<IRoleRepository,EfRoleRepository>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<YemekTarifleriContext>(
    options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 41)))
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options =>
{
    Options.AccessDeniedPath = "/Home/NotFoundPage";
    Options.LoginPath = "/Home/NotFoundPage";
});

builder.Services.AddAuthorization(Options =>
{
    Options.AddPolicy("isLogin", policy => policy.Requirements.Add(new isLoginRequirement()));
    Options.AddPolicy("isOwner", policy => policy.Requirements.Add(new isOwnerRequirement()));
    Options.AddPolicy("isAdmin", policy => policy.Requirements.Add(new isAdminRequirement()));

});

builder.Services.AddScoped<IAuthorizationHandler, İsLoginHandler>();
builder.Services.AddScoped<IAuthorizationHandler, isOwnerHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorizationHandler, isAdminHandler>();



var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

SeedData.TestVerileriDoldur(app);

app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "FoodRemove",
    pattern: "remove/{url}",
    defaults: new { controller = "Food", action = "RemoveFood" }
);

app.MapControllerRoute(
    name: "yemek_turleri",
    pattern: "yemekler/{tur}",
    defaults: new { controller = "Home", action = "Yemek_turu" }
);

app.MapControllerRoute(
    name: "yemek_detay",
    pattern: "yemek/{url}",
    defaults: new { controller = "Home", action = "Details" }
);

app.MapControllerRoute(
    name: "yemek_edit",
    pattern: "foodedit/{url}",
    defaults: new { controller = "Food", action = "FoodEdit" }
);

app.MapControllerRoute(
    name: "kullanici",
    pattern: "user/{username}",
    defaults: new { controller = "Users", action = "UserPage" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();