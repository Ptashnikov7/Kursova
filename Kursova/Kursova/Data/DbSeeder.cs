using Kursova.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Kursova.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Create roles
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create admin user
            var adminUser = await userManager.FindByEmailAsync("admin@grocery.com");
            if (adminUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin@grocery.com",
                    Email = "admin@grocery.com",
                    EmailConfirmed = true
                };
                var createPowerUser = await userManager.CreateAsync(user, "Admin123!");
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // Seed Products
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Свежие яблоки",
                        Description = "Хрустящие и сочные яблоки, идеальны для перекуса.",
                        Price = 120.50m,
                        ImageUrl = "https://images.unsplash.com/photo-1560806887-1e4cd0b6faa6?auto=format&fit=crop&w=500&q=80"
                    },
                    new Product
                    {
                        Name = "Хлеб цельнозерновой",
                        Description = "Свежеиспеченный полезный хлеб.",
                        Price = 85.00m,
                        ImageUrl = "https://images.unsplash.com/photo-1509440159596-0249088772ff?auto=format&fit=crop&w=500&q=80"
                    },
                    new Product
                    {
                        Name = "Молоко фермерское",
                        Description = "Натуральное молоко жирностью 3.2%.",
                        Price = 90.00m,
                        ImageUrl = "https://images.unsplash.com/photo-1550583724-b2692b85b150?auto=format&fit=crop&w=500&q=80"
                    },
                    new Product
                    {
                        Name = "Сыр Чеддер",
                        Description = "Полутвердый сыр с насыщенным вкусом.",
                        Price = 350.00m,
                        ImageUrl = "https://images.unsplash.com/photo-1618164436241-4473940d1fce?auto=format&fit=crop&w=500&q=80"
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
