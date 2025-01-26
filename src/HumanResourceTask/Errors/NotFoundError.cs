namespace HumanResourceTask.Errors
{
    public class NotFoundError : FluentResults.Error
    {
        public NotFoundError(string resourceType, Guid id) : base($"A {resourceType} with id {id} was not found.")
        {
        }
    }
}
