using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<BlogContext>(options =>
{
        var config = builder.Configuration;
        var connectionString = config.GetConnectionString("sql_connection");
        options.UseSqlite(connectionString);
});



builder.Services.AddScoped<IPostRepository, EfPostRepository>();
builder.Services.AddScoped<ITagRepository, EfTagRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();


var app = builder.Build();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
SeedData.TestVerileriniDoldur(app);

app.MapControllerRoute(
        name: "post_details",
        pattern: "post/details/{url}", defaults: new { controller = "Posts", action = "Details" }
                       );

app.MapControllerRoute(
        name: "post_edit",
        pattern: "post/edit/{url}", defaults: new { controller = "Posts", action = "Edit" }
    );

app.MapControllerRoute(
        name: "posts_by_tag",
        pattern: "post/tag/{tag}", defaults: new { controller = "Posts", action = "Index" }
                       );
app.MapControllerRoute(
    name: "user_profile",
    pattern: "User/Profile/{userUrl}",
    defaults: new { controller = "User", action = "Profile" }
);


app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Posts}/{action=Index}/{id?}"
                       );

app.Run();
