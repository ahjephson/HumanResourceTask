namespace HumanResourceTask.Api.Dto.Employee
{
    public record ListEmployeesResponse
    {
        public IReadOnlyList<EmployeeListItem> Items { get; init; } = [];

        public long Total { get; init; }

        public bool HasMore { get; init; }
    }
}
