namespace OrfeaBG.Data
{
    using static DataConstants;
    using Microsoft.AspNetCore.Identity;
    using System.Linq;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using OrfeaBG.Data.Models;
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Tasks;

    public static class DbInitializer
    {
        public static void Initialize(OrfeaBGDbContext context, IServiceProvider provider)
        {
            context.Database.EnsureCreated();

            var userManager = provider.GetService<UserManager<User>>();
            var roleManager = provider.GetService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {

                    var roles = new[]
                    {
                            AdminRole,
                            BlogAuthorRole,
                            RecipeRole
                    };

                    foreach (var role in roles)
                    {
                        var roleExists = await roleManager.RoleExistsAsync(role);

                        if (!roleExists)
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = role
                            });
                        }
                    }

                    var adminEmail = "admin@mysite.com";
                    var adminName = "admin@mysite.com";

                    var adminUser = await userManager.FindByEmailAsync(adminEmail);

                    if (adminUser == null)
                    {
                        adminUser = new User
                        {
                            Email = adminEmail,
                            UserName = adminName,
                        };

                        await userManager.CreateAsync(adminUser, "admin12");

                        foreach (var role in roles)
                        {
                            await userManager.AddToRoleAsync(adminUser, role);
                        }
                    }
                })
                .Wait();

            context.SaveChanges();
        }
    }
}
