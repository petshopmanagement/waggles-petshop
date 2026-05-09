namespace PetManagementSystem.Api.Exceptions
{
    public class VaccinationValidationException : Exception
    {
        public VaccinationValidationException(string message)
            : base(message)
        {
        }
    }
}
