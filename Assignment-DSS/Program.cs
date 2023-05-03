using Assignment_DSS.Interfaces;
using Assignment_DSS.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Assignment_DSS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IPostsRep, PostsRepo>();
            builder.Services.AddScoped<IUserRepo, UsersRepo>();
            builder.Services.AddScoped<ICommentsRep, CommentsRepo>();
           

            builder.Services.AddDbContext<data.ApplicationDBContext>(options => {
                var connString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connString);
            });

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10000);
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Home}/{id?}");

            app.MapControllerRoute(
            name: "Login",
            pattern: "{controller=Login}/{action=Login}/{id?}");   
            
            app.MapControllerRoute(
            name: "Reg",
            pattern: "{controller=Reg}/{action=Reg}/{id?}");

            app.MapControllerRoute(
            name: "Add",
            pattern: "{controller=Add}/{action=Add}/{id?}");

            app.MapControllerRoute(
            name: "Detail",
            pattern: "{controller=Home}/{action=Post}/{id?}");

            app.Run();
        }
    }
}