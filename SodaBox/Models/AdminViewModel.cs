using System.Collections.Generic;
using SodaBox.DataAccess.Entities;

namespace SodaBox.Models
{
    public class AdminViewModel
    {
        public IEnumerable<Drink> drinks { get; set; }
    }
}
