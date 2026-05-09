namespace PetManagementSystem.Api.Exceptions;

public class InvalidRoleException : Exception
{
    public InvalidRoleException(string message = "Invalid role specified.")
        : base(message)
    {
    }
}