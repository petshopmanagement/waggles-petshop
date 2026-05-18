namespace PetManagementSystem.Api.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; set; }

        public ValidationException(string error)
            : base(error)
        {
            Errors = new List<string>
            {
                error
            };
        }

        public ValidationException(List<string> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}
