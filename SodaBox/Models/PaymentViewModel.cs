namespace SodaBox.Models
{
    public class PaymentViewModel
    {
        public int totalAmount { get; set; } // требуемая сумма
        public Dictionary<int, int> coins { get; set; } = new Dictionary<int, int>
        {
            { 1, 0 },
            { 2, 0 },
            { 5, 0 },
            { 10, 0 }
        };
        public int currentAmount => coins.Sum(c => c.Key * c.Value);
        public bool isCanPay => currentAmount - totalAmount >= 0;
    }
}