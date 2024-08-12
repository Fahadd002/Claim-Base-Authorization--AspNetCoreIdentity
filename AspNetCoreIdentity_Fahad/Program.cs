using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNetCoreIdentity_Fahad.Data;
using AspNetCoreIdentity_Fahad.Models;
using System;

namespace AspNetCoreIdentity_Fahad
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext <ApplicationDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("security")));
         

            builder.Services.AddIdentity<ApplicationUser,ApplicationRole>(op=>
            {
                op.Password.RequiredLength = 6;
                op.Password.RequireDigit = false;
                op.Password.RequireNonAlphanumeric = false;
                op.Password.RequireUppercase = false;
                op.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
          
            builder.Services.AddAuthorization(opts => {
                opts.AddPolicy("DeleteRolePolicy", p => p.RequireClaim("Delete Role")
                                                         .RequireClaim("Edit Role"));
                                                    
            });

            builder.Services.AddAuthorization(opts => {
                opts.AddPolicy("DeleteRolePolicy", p => p.RequireClaim("Delete Role") ) ; 
                opts.AddPolicy("EditRolePolicy", p => p.RequireClaim("Edit Role") ) ;

            });
        
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapDefaultControllerRoute();
            app.Run();
        }
    }
}
