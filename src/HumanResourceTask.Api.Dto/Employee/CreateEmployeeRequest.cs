using System.Diagnostics.CodeAnalysis;

namespace HumanResourceTask.Api.Dto.Employee
{
    public record CreateEmployeeRequest
    {
        [NotNull]
        public string? FirstName { get; set; }

        [NotNull]
        public string? LastName { get; set; }

        [NotNull]
        public string? Email { get; set; }

        [NotNull]
        public DateOnly? DateOfBirth { get; set; }

        [NotNull]
        public Guid? DepartmentId { get; set; }

        [NotNull]
        public Guid? StatusId { get; set; }

        [NotNull]
        public long? EmployeeNumber { get; set; }
    }
}
