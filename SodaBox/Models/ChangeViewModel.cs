namespace SodaBox.Models
{
    public class ChangeViewModel
    {
        public int totalAmount {  get; set; }
        public int currentAmount {  get; set; }
        public int change => currentAmount - totalAmount;
    }
}
