using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using st10209886_PROG_POE1.Models;

namespace st10209886_PROG_POE1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ClaimContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ClaimContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Seed roles and users
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedRolesAndUsers(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Enable authentication middleware
            app.UseAuthorization(); // Enable authorization middleware

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }

        private static async Task SeedRolesAndUsers(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Define roles
            string[] roles = { "Lecturer", "Coordinator", "HR" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create users and assign roles
            var users = new[]
            {
        new { Email = "kiml@gmail.com", Password = "Password123!", Role = "Lecturer" },
        new { Email = "kimc@gmail.com", Password = "Password123!", Role = "Coordinator" },
        new { Email = "kimhr@gmail.com", Password = "Password123!", Role = "HR" }
    };

            foreach (var userInfo in users)
            {
                // Check if a user with the current email already exists
                var existingUser = await userManager.FindByEmailAsync(userInfo.Email);
                if (existingUser == null)
                {
                    var user = new IdentityUser { UserName = userInfo.Email, Email = userInfo.Email };
                    var result = await userManager.CreateAsync(user, userInfo.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userInfo.Role);
                    }
                }
                else
                {
                    // If the user exists but the email is different, update it
                    existingUser.UserName = userInfo.Email;
                    existingUser.Email = userInfo.Email;
                    await userManager.UpdateAsync(existingUser);

                    // Ensure the user is in the correct role
                    if (!await userManager.IsInRoleAsync(existingUser, userInfo.Role))
                    {
                        await userManager.AddToRoleAsync(existingUser, userInfo.Role);
                    }
                }
            }
        }

    }
}
