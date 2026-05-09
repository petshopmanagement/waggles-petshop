namespace PetManagementSystem.Api.DTOs
{
    public class FoodDTO
    {
        public int FoodId { get; set; }

        public string? Name { get; set; }

        public string? Brand { get; set; }

        public string? Type { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
    }

    public class CreatePetFoodDto
    {
        public string? Name { get; set; }

        public string? Brand { get; set; }

        public string? Type { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
    }

    public class UpdatePetFoodDto
    {
        public string? Name { get; set; }

        public string? Brand { get; set; }

        public string? Type { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
    }

}
