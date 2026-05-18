namespace PetManagementSystem.Api.Exceptions
{
    public class VaccinationNotFoundException : Exception
    {
        public VaccinationNotFoundException(string message)
            : base(message)
        {
        }
    }
}
