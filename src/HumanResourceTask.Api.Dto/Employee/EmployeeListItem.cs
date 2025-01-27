namespace HumanResourceTask.Api.Dto.Employee
{
    public record EmployeeListItem
    {
        public Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public DateOnly DateOfBirth { get; init; }
        public required string DepartmentName { get; init; }
        public required string StatusName { get; init; }
        public long EmployeeNumber { get; init; }
    }
}
