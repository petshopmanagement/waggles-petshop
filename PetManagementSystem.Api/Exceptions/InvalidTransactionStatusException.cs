namespace PetManagementSystem.Api.Exceptions;

public class InvalidTransactionStatusException : Exception
{
    private static readonly string[] _allowed = ["Pending", "Success", "Failed"];

    public InvalidTransactionStatusException()
        : base($"Transaction status must be one of: {string.Join(", ", _allowed)}.") { }

    public InvalidTransactionStatusException(string provided)
        : base($"'{provided}' is not a valid transaction status. Allowed values: {string.Join(", ", _allowed)}.") { }
}
