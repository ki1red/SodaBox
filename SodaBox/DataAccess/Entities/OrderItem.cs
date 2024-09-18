using System.ComponentModel.DataAnnotations;

namespace SodaBox.DataAccess.Entities
{
    public class OrderItem
    {
        [Key]
        public int id { get; set; } // Уникальный идентификатор элемента заказа

        [Required]
        [StringLength(100)]
        public string brandName { get; set; } // Бренд напитка

        [Required]
        [StringLength(100)]
        public string drinkName { get; set; } // Название напитка

        [Required]
        [Range(1, int.MaxValue)]
        public int quantity { get; set; } // Количество заказанных единиц

        [Required]
        [Range(0, double.MaxValue)]
        public double price { get; set; } // Цена одной единицы
    }
}
