namespace PetManagementSystem.Api.Exceptions;

public class TransactionNotFoundException : Exception
{
    public TransactionNotFoundException()
        : base("Transaction not found.") { }

    public TransactionNotFoundException(int id)
        : base($"Transaction with ID {id} was not found.") { }
}
