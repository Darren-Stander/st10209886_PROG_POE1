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
                new { Email = "lecturer@example.com", Password = "Password123!", Role = "Lecturer" },
                new { Email = "coordinator@example.com", Password = "Password123!", Role = "Coordinator" },
                new { Email = "hr@example.com", Password = "Password123!", Role = "HR" }
            };

            foreach (var userInfo in users)
            {
                if (await userManager.FindByEmailAsync(userInfo.Email) == null)
                {
                    var user = new IdentityUser { UserName = userInfo.Email, Email = userInfo.Email };
                    var result = await userManager.CreateAsync(user, userInfo.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userInfo.Role);
                    }
                }
            }
        }
    }
}
