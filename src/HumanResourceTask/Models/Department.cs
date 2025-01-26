namespace HumanResourceTask.Models
{
    public record Department
    {
        public Guid Id { get; init; }

        public required string Name { get; init; }
    }
}
