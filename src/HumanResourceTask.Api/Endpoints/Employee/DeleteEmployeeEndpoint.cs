using HumanResourceTask.Api.Authentication;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Services;

namespace HumanResourceTask.Api.Endpoints.Employee
{
    public class DeleteEmployeeEndpoint : ResultHandlingEndpoint<DeleteEmployeeRequest>
    {
        private readonly IEmployeeService _employeeService;

        public DeleteEmployeeEndpoint(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public override void Configure()
        {
            Delete("/employee/{id}");
            Policies(PolicyNames.DeleteEmployee);
        }

        public override async Task HandleAsync(DeleteEmployeeRequest req, CancellationToken ct)
        {
            var result = await _employeeService.DeleteEmployeeAsync(req.Id);

            if (!result.IsSuccess)
            {
                await HandleFailureAsync(result, ct);
            }
            else
            {
                await SendNoContentAsync(ct);
            }
        }
    }
}
