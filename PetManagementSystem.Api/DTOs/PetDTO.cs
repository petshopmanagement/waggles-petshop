namespace PetManagementSystem.Api.DTOs
{   
    public class PetDto
    {
        public int PetId { get; set; }
        public string? Name { get; set; }
        public string? Breed { get; set; }
        public string? Description { get; set; }
        public int? Age { get; set; }
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
    }
    public class PetCreate   
    {
        public string? Name { get; set; }
        public string? Breed { get; set; }
        public string? Description { get; set; }
        public int? Age { get; set; }
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
    }
    public class PetUpdate  
    {
        public string? Name { get; set; }
        public string? Breed { get; set; }
        public string? Description { get; set; }
        public int? Age { get; set; }
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
