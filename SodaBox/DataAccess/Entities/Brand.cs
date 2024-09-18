using System.ComponentModel.DataAnnotations;

namespace SodaBox.DataAccess.Entities
{
    public class Brand
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        public List<Drink> drinks { get; set; }
    }
}
