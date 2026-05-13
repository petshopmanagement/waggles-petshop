namespace PetManagementSystem.Web.Models
{
    public class PetViewModel
    {
        public int PetId { get; set; }
        public string? Name { get; set; }
        public string? Breed { get; set; }
        public int? Age { get; set; }
        public decimal? Price { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
    }
}
