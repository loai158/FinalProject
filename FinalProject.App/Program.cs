using FinalProject.Core;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FinalProject.Services;
using FinalProject.Infrastructure;
namespace FinalProject.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.
                 AddCoreDependences()
                .AddInfrastructureDependences()
                .AddServicesDependences();
            builder.Services.AddDbContext<ApplicationDbContext>(
               option =>
               {
                   option.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
               }
               );
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
             options =>
             {
                 options.SignIn.RequireConfirmedEmail = true;
                 options.Password.RequireUppercase = false;
                 options.Password.RequireLowercase = false;
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequiredLength = 6;

                 options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                 options.Lockout.MaxFailedAccessAttempts = 5;
                 options.Lockout.AllowedForNewUsers = true;
             }).AddEntityFrameworkStores<ApplicationDbContext>()
                             .AddDefaultTokenProviders();






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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
