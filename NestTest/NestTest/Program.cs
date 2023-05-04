using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NestTest.DAL;
using NestTest.Models;
using NestTest.Services;
using System.Drawing;

namespace NestTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
           
            builder.Services.AddDbContext<NestDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            //builder.Services.AddIdentity<AppUser, IdentityRole>(con =>
            //{
            //    con.Password.RequiredLength = 8;
            //    con.Password.RequireNonAlphanumeric = false;
            //    con.Password.RequireDigit = true;
            //    con.Password.RequireLowercase = true;
            //    con.Password.RequireUppercase = true;
            //    con.User.RequireUniqueEmail = true;
            //    con.Lockout.AllowedForNewUsers = true;
            //    con.Lockout.MaxFailedAccessAttempts= 5;
            //    con.Lockout.DefaultLockoutTimeSpan= TimeSpan.FromSeconds(30);
            //}).AddDefaultTokenProviders()
            //   .AddEntityFrameworkStores<NestDbContext>();
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<NestDbContext>()
                            .AddDefaultTokenProviders();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireDigit = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequireUppercase = true;
                //options.User.RequireUniqueEmail = true;
                //options.Lockout.AllowedForNewUsers = true;
                //options.Lockout.MaxFailedAccessAttempts = 5;
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
            });
            //builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddScoped<_LayoutServices>();

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}