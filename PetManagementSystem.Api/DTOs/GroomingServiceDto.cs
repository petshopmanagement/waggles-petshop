namespace PetManagementSystem.Api.DTOs
{
    public class GroomingServiceDto
    {
        public int ServiceId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public byte? Available { get; set; }
    }
}
