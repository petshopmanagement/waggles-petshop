namespace PetManagementSystem.Api.Helpers;

public static class TransactionHelper
{
    public static readonly string[] ValidStatuses = ["Pending", "Success", "Failed"];

    public static bool IsValidStatus(string? status)
        => ValidStatuses.Contains(status);

    public static bool IsSuccess(string? status)
        => status == "Success";

    public static bool IsFailed(string? status)
        => status == "Failed";

    public static bool IsPending(string? status)
        => status == "Pending";
}
