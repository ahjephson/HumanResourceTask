using FluentResults;
using HumanResourceTask.Api.Dto.Department;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Api.Dto.Status;

namespace HumanResourceTask.Web.Services
{
    public interface IApiClient
    {
        Task<Result<EmployeeResponse>> CreateEmployeeAsync(CreateEmployeeRequest request);

        Task<Result> DeleteEmployeeAsync(Guid id);

        Task<Result<IEnumerable<DepartmentListItem>>> GetDepartmentsAsync();

        Task<Result<EmployeeResponse>> GetEmployeeAsync(Guid id);

        Task<Result<IEnumerable<StatusListItem>>> GetStatusesAsync();

        Task<Result<ListEmployeesResponse>> ListEmployeesAsync(ListEmployeesRequest request);

        Task<Result<EmployeeResponse>> UpdateEmployeeAsync(UpdateEmployeeRequest request);

        string GetFriendlyError(ErrorResponse errorResponse);
    }
}
