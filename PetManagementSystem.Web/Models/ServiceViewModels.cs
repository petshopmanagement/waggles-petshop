using System.ComponentModel.DataAnnotations;

namespace PetManagementSystem.Web.Models
{
    public class GroomingServiceViewModel
    {
        public int ServiceId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;
    }

    public class VaccinationViewModel
    {
        public int VaccinationId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;
    }

    public class AdminServicesViewModel
    {
        public IEnumerable<GroomingServiceViewModel> GroomingServices { get; set; } = new List<GroomingServiceViewModel>();
        public IEnumerable<VaccinationViewModel> Vaccinations { get; set; } = new List<VaccinationViewModel>();
    }
}
