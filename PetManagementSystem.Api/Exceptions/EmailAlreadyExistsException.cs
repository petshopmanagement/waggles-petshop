namespace PetManagementSystem.Api.Exceptions;

public class EmailAlreadyExistsException : Exception
{
    public EmailAlreadyExistsException(string message = "Email already exists.")
        : base(message)
    {
    }
}