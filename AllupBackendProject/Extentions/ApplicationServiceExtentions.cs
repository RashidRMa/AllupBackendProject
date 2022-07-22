using AllupBackendProject.DAL;
using AllupBackendProject.Helpers;
using AllupBackendProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AllupBackendProject.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;

                opt.User.RequireUniqueEmail = true;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.AllowedForNewUsers = true;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            return services;
        }
    }
}
