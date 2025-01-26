namespace HumanResourceTask.Errors
{
    public class ValidationError : FluentResults.Error
    {
        public ValidationError(string? propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }

        public string? PropertyName { get; init; }
    }
}
