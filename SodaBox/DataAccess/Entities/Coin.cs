using System.ComponentModel.DataAnnotations;

namespace SodaBox.DataAccess.Entities
{
    public class Coin
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int quantity { get; set; }

        [Required]
        [StringLength(500)]
        public string imagePath { get; set; }
    }
}
