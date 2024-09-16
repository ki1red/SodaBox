using System.Collections.Generic;

namespace SodaBox.Models
{
    public class PaymentViewModel
    {
        public decimal TotalAmount { get; set; }
        public Dictionary<int, int> Coins { get; set; } = new Dictionary<int, int>
        {
            { 1, 0 },
            { 2, 0 },
            { 5, 0 },
            { 10, 0 }
        };
        public decimal CurrentAmount => Coins.Sum(c => c.Key * c.Value);
        public decimal Change => CurrentAmount - TotalAmount;
        public bool CanPay => Change >= 0;
    }
}
