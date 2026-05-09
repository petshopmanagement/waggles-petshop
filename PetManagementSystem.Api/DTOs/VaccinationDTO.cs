namespace PetManagementSystem.Api.DTOs
{
    public class VaccinationDTO
    {
        public int VaccinationId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public bool? Available { get; set; }
    }
}
