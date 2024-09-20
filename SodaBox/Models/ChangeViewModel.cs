using SodaBox.DataAccess.Entities;

namespace SodaBox.Models
{
    public class ChangeViewModel
    {
        public int totalAmount {  get; set; }
        public int currentAmount {  get; set; }
        public List<Coin> coins { get; set; }
        public int change => currentAmount - totalAmount;
    }
}
