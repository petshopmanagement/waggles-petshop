namespace PetManagementSystem.Api.DTOs.GroomingServiceDtos
{
    public class UpdateGroomingServiceDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public byte? Available { get; set; }
    }
}
