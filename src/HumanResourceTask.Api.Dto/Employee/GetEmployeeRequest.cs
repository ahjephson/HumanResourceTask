namespace HumanResourceTask.Api.Dto.Employee
{
    public record GetEmployeeRequest
    {
        public Guid Id { get; init; }
    }
}
