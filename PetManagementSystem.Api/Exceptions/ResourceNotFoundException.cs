namespace PetManagementSystem.Api.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public string ResourceName { get; set; }

        public object ResourceValue { get; set; }

        public ResourceNotFoundException(
            string resourceName,
            object resourceValue)

            : base($"{resourceName} with value '{resourceValue}' was not found.")
        {
            ResourceName = resourceName;
            ResourceValue = resourceValue;
        }
    }
}
