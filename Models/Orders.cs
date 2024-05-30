using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CafeKDSApi.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        // Navigation property for Items
        public List<Item> Items { get; set; } = new List<Item>();
    }

    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        // Foreign key for Order
        public int OrderId { get; set; }

        // Additional properties specific to an Item
        // Example: DrinkType, DrinkSize, MilkChoice, etc.
        [Required]
        [StringLength(20)]
        public string DrinkType { get; set; } = "Latte";

        [Required]
        [StringLength(10)]
        public string DrinkSize { get; set; } = "Small";

        [Required]
        [StringLength(10)]
        public string MilkChoice { get; set; } = "FullCream";

        [StringLength(10)]
        public string Flavour { get; set; } = string.Empty;

        // Options to alter the drink such as strength and quantity
        public string[] Comment { get; set; } = new string[0];

        [Required]
        public bool IsMade { get; set; } = false;

        [Required]
        public DateTime OrderTime { get; set; }
    }
}
