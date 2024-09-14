using SodaBox.DataAccess.Entities;
using System.Collections.Generic;

namespace SodaBox.Models
{
    public class StoreViewModel
    {
        // Список брендов для фильтрации
        public List<Brand> brands { get; set; }

        // Список напитков, которые должны отображаться
        public List<Drink> drinks { get; set; }

        // Параметры для фильтрации
        public int? selectedBrandId { get; set; }
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }
    }
}
