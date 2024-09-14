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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Начальные данные для таблицы Brand
            modelBuilder.Entity<Brand>().HasData(
                new Brand { id = 1, name = "Coca Cola" },
                new Brand { id = 2, name = "Pepsi" },
                new Brand { id = 3, name = "Fanta" },
                new Brand { id = 4, name = "Sprite" }
            );

            // Начальные данные для таблицы Drink
            modelBuilder.Entity<Drink>().HasData(
                new Drink { id = 1, name = "Coca Cola", price = 100, quantity = 10, brandId = 1, imagePath = "images/drinks/coca_cola.png" },
                new Drink { id = 2, name = "Pepsi", price = 95, quantity = 5, brandId = 2, imagePath = "images/drinks/pepsi.png" },
                new Drink { id = 3, name = "Coca Cola Zero", price = 130, quantity = 5, brandId = 1, imagePath = "images/drinks/coca_cola_zero.png" },
                new Drink { id = 4, name = "Fanta", price = 120, quantity = 5, brandId = 3, imagePath = "images/drinks/fanta.png" },
                new Drink { id = 5, name = "Sprite", price = 120, quantity = 5, brandId = 4, imagePath = "images/drinks/sprite.png" },
                new Drink { id = 6, name = "Sprite", price = 120, quantity = 0, brandId = 4, imagePath = "images/drinks/sprite.png" }
            );
        }
    }
}
