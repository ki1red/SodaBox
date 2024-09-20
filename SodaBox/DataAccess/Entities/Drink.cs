using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SodaBox.DataAccess.Entities
{
    public class Drink
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int quantity { get; set; }

        [ForeignKey("brand")]
        public int brandId { get; set; }      // Внешний ключ
        public Brand brand { get; set; }      // Навигационное свойство

        [Required]
        [StringLength(500)]
        public string imagePath { get; set; }

    }
}
