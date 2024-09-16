using SodaBox.DataAccess.Entities;
using System.Collections.Generic;
using SodaBox.Services.Classes;

namespace SodaBox.Models
{
    public class BucketViewModel
    {
        public List<CartItem> cartItems { get; set; }
        public int sum {  get; set; }
    }
}
