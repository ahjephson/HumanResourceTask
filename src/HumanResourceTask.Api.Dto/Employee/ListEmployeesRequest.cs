namespace HumanResourceTask.Api.Dto.Employee
{
    public record ListEmployeesRequest
    {
        public Guid? StatusId { get; init; }

        public Guid? DepartmentId { get; init; }

        public Sort? Sort { get; init; }

        public int? Offset { get; init; }

        public int? Limit { get; init; }
    }
}
