using FinalProject.App.Helper.EmailSettings;
using FinalProject.App.Utility.EmailSettings;
using FinalProject.App.Utility.StripeSettings;
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
using Stripe;
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

            //stripe services
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
            //send email
             builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSetting"));

            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
             builder.Services.AddScoped<IPatientRepositry ,PatientRepositry >();
             builder.Services.AddScoped<IDoctorRepositry ,DoctorRepositry >();
             builder.Services.AddScoped<IRegisterApplyRepositoey, RegisterApplyRepositoey>();
             builder.Services.AddScoped<INurseRepositry ,NurseRepositry >();
             builder.Services.AddScoped<IEmailSettings,EmailSettings>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

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
