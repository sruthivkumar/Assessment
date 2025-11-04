using Microsoft.EntityFrameworkCore;
using ProductOrderAPI.Model;

namespace ProductOrderAPI.Data
{
    public static class SeedData

    {
        public static void EnsureSeedData(AppDbContext db, IConfiguration config)
        {
            db.Database.Migrate();
            if (!db.Users.Any(u => u.Email == "admin@demo.com"))
            {
                var admin = new User
                {
                    FullName = "Admin User",
                    Email = "admin@demo.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd!"),
                    Role = "Admin"
                };
                db.Users.Add(admin);
            }
            if (!db.Products.Any())
            {
                db.Products.AddRange(
                new Product { Name = "Pen", Price = 1.50m, Stock = 100 },
                new Product { Name = "Notebook", Price = 3.00m, Stock = 50 },
                new Product { Name = "Backpack", Price = 25.00m, Stock = 20 }
                );
            }
            db.SaveChanges();
        }
    }
}