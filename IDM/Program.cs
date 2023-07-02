/*
Title       :Identity Management System
Author      :Harikrishnan G
Created at  :27-02-2023
Updated at  :31-05-2023
Reviewed by :
Reviewed at : 
*/

using IDM.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IDM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            #endregion

            builder.Services.AddDefaultIdentity<IdentityUser>(
                options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            
            #region Configure Sessions
            builder.Services.AddSession(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                    options.IdleTimeout = TimeSpan.FromMinutes(30);
                });
            #endregion
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();  

            using(var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Employee" }; 
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // using (var scope = app.Services.CreateScope())
            // {
            //     var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //     string email = "@User.Identity?.Name";
            //     string password = "Aspire@123";

            //     if(await userManager.FindByEmailAsync(email)== null)
            //     {
            //         var user = new IdentityUser();
            //         user.Email = email;
            //         await userManager.CreateAsync(user,password);

            //         await userManager.AddToRoleAsync(user, "Employee");
            //     }
            // }

            app.Run();


        }
    }
}