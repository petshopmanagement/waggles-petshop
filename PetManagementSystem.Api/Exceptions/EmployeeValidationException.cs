namespace PetManagementSystem.Api.Exceptions
{
    public class EmployeeValidationException :Exception
    {
        public EmployeeValidationException(string message)
            : base(message)
        {
        }
    }
}
