using System.ComponentModel.DataAnnotations;

namespace PetManagementSystem.Web.Models
{
    public class PetFoodViewModel
    {
        public int FoodId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Range(0, 10000)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; }
    }
}
