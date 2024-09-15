using SodaBox.DataAccess.Entities;
using System.Collections.Generic;

namespace SodaBox.Models
{
    public class BucketViewModel
    {
        public List<CartItem> cartItems { get; set; }
    }

    public class CartItem
    {
        public Drink drink { get; set; }
        public int quantity { get; set; }
    }
}
