using FinalProject.Core;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Infrastructure;
using FinalProject.Infrastructure.DataAccess;
using FinalProject.Infrastructure.IRepositry;
using FinalProject.Infrastructure.Repositry;
using FinalProject.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Authentication.Google;

using Microsoft.EntityFrameworkCore;
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
                 options.Password.RequireUppercase = false;
                 options.Password.RequireLowercase = false;
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequiredLength = 6;
             }).AddEntityFrameworkStores<ApplicationDbContext>()
                             .AddDefaultTokenProviders();


            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            //external Login with google

            //          builder.Services.AddAuthentication(options =>
            //          {
            //              options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
            //              options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //          }).AddGoogle(options =>
            //{
            //    IConfigurationSection GoogleAuth = builder.Configuration.GetSection("Authentication:Google");
            //    options.ClientId = GoogleAuth["ClientId"];
            //    options.ClientSecret = GoogleAuth["ClientSecret"];
            //});
            builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)  
                        .AddGoogle(options =>
                        {
                            IConfigurationSection googleAuth = builder.Configuration.GetSection("Authentication:Google");
                            options.ClientId = googleAuth["ClientId"];
                            options.ClientSecret = googleAuth["ClientSecret"];
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


            app.MapControllerRoute(

                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
