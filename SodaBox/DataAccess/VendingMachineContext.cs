using Microsoft.EntityFrameworkCore;
using SodaBox.DataAccess.Entities;

namespace SodaBox.DataAccess
{
    public class VendingMachineContext : DbContext
    {
        public VendingMachineContext(DbContextOptions<VendingMachineContext> options) : base(options)
        {
        }

        public DbSet<Drink> drinks { get; set; }
        public DbSet<Brand> brands { get; set; }
        public DbSet<Coin> coins { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Начальные данные для таблицы Brand
            modelBuilder.Entity<Brand>().HasData(
                new Brand { id = 1, name = "Coca Cola" },
                new Brand { id = 2, name = "Pepsi" },
                new Brand { id = 3, name = "Fanta" },
                new Brand { id = 4, name = "Sprite" },
                new Brand { id = 5, name = "Dr Pepper" }
            );

            // Начальные данные для таблицы Drink
            modelBuilder.Entity<Drink>().HasData(
                new Drink { id = 1, name = "Coca Cola", price = 65, quantity = 3, brandId = 1, imagePath = "images/drinks/coca_cola.png" },
                new Drink { id = 2, name = "Coca Cola Zero", price = 70, quantity = 0, brandId = 1, imagePath = "images/drinks/coca_cola_zero.png" },
                new Drink { id = 3, name = "Pepsi", price = 40, quantity = 3, brandId = 2, imagePath = "images/drinks/pepsi.png" },
                new Drink { id = 4, name = "Pepsi Zero", price = 45, quantity = 3, brandId = 2, imagePath = "images/drinks/pepsi_zero.png" },
                new Drink { id = 5, name = "Fanta", price = 55, quantity = 2, brandId = 3, imagePath = "images/drinks/fanta.png" },
                new Drink { id = 6, name = "Sprite", price = 55, quantity = 0, brandId = 4, imagePath = "images/drinks/sprite.png" },
                new Drink { id = 7, name = "Dr Pepper", price = 65, quantity = 1, brandId = 5, imagePath = "images/drinks/dr_pepper.png" },
                new Drink { id = 8, name = "Dr Pepper Cherry", price = 40, quantity = 1, brandId = 5, imagePath = "images/drinks/dr_pepper_cherry.png" }
            );

            modelBuilder.Entity<Coin>().HasData(
                new Coin { id = 1, price = 1, quantity = 20, imagePath = "images/coins/1.png" },
                new Coin { id = 2, price = 2, quantity = 20, imagePath = "images/coins/2.png" },
                new Coin { id = 3, price = 5, quantity = 20, imagePath = "images/coins/5.png" },
                new Coin { id = 4, price = 10, quantity = 20, imagePath = "images/coins/10.png" }
            );
        }
    }
}
