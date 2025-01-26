namespace HumanResourceTask.Api.Dto.Department
{
    public record ListDepartmentsResponse
    {
        public ListDepartmentsResponse(IReadOnlyList<DepartmentListItem> items)
        {
            Items = items;
        }

        public IReadOnlyList<DepartmentListItem> Items { get; }
    }
}
