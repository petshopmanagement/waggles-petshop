namespace PetManagementSystem.Api.DTOs
{
    public class VaccinationDto
    {
        public int VaccinationId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public bool? Available { get; set; }
    }

    public class CreateVaccinationDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public bool? Available { get; set; }
    }

    public class UpdateVaccinationDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public bool? Available { get; set; }
    }

}