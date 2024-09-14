namespace SodaBox.DataAccess.Entities
{
    public class Drink
    {
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }

        public int brandId { get; set; }      // Внешний ключ
        public Brand brand { get; set; }      // Навигационное свойство

        public string imagePath { get; set; }
    }
}
