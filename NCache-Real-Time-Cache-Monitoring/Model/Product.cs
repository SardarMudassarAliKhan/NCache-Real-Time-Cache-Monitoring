using System.ComponentModel.DataAnnotations;

namespace NCache_Real_Time_Cache_Monitoring.Model
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

}
