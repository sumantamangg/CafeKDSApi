using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CafeKDSApi.Models
{
    public class Orders
    {
 
        [Key]
        public int OrderID { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string DrinkType { get; set; } = "Latte";

        [Required]
        [StringLength(10)]
        public string DrinkSize { get; set; } = "Small";

        [Required]
        [StringLength(10)]
        public string MilkChoice { get; set; } = "FullCream";

        [Range(0, 5)]
        public int Shots { get; set; }

        [StringLength(10)]
        public string Strength { get; set; } = String.Empty;

        [Required]
        public DateTime OrderTime { get; set; }

    }

    public enum DrinkType
    {
        Latte,
        Cappuccino,
        FlatWhite
    }
    
}
