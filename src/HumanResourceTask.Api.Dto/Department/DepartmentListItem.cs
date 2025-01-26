namespace HumanResourceTask.Api.Dto.Department
{
    public record DepartmentListItem
    {
        public Guid Id { get; init; }

        public required string Name { get; set; }
    }
}
