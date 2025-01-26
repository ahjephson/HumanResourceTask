namespace HumanResourceTask.Api.Dto.Employee
{
    public record UpdateEmployeeRequest
    {
        public required Guid Id { get; init; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public Guid? DepartmentId { get; set; }

        public Guid? StatusId { get; set; }

        public long? EmployeeNumber { get; set; }
    }
}
