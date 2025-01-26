namespace HumanResourceTask.Models
{
    public record EmployeeRecord
    {
        public Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public DateOnly DateOfBirth { get; init; }
        public Guid DepartmentId { get; init; }
        public Guid StatusId { get; init; }
        public long EmployeeNumber { get; init; }
        public DateTimeOffset CreatedAtUtc { get; init; }
        public DateTimeOffset? UpdatedAtUtc { get; init; }
        public bool Deleted { get; init; }
    }
}
