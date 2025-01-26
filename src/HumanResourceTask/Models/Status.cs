namespace HumanResourceTask.Models
{
    public record Status
    {
        public Guid Id { get; init; }

        public required string Name { get; init; }
    }
}
