namespace PetManagementSystem.Api.Exceptions;

public class AppException : Exception
{
    public AppException(string message = "Application error occurred.")
        : base(message)
    {
    }
}