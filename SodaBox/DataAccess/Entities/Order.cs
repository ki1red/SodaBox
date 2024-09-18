using System.ComponentModel.DataAnnotations;

namespace SodaBox.DataAccess.Entities
{
    public class Order
    {
        [Key]
        public int id { get; set; } // Уникальный идентификатор заказа

        [Required]
        public DateTime orderDate { get; set; } // Дата заказа

        public List<OrderItem> orderItems { get; set; } = new List<OrderItem>(); // Состав заказа

        [Required]
        [Range(0, int.MaxValue)]
        public int totalAmount { get; set; }
    }
}
