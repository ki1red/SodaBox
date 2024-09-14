namespace SodaBox.DataAccess.Entities
{
    public class Brand
    {
        public int id { get; set; }
        public string name { get; set; }

        public List<Drink> drinks { get; set; }    // Навигационное свойство
    }
}
