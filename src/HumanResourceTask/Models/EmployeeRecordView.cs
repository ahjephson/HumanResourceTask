namespace HumanResourceTask.Models
{
    public record EmployeeRecordView : EmployeeRecord
    {
        public required string DepartmentName { get; init; }

        public required string StatusName { get; init; }
    }
}
