namespace PetManagementSystem.Api.DTOs.GroomingServiceDtos
{
    public class CreateGroomingServiceDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public byte? Available { get; set; }
    }
}
