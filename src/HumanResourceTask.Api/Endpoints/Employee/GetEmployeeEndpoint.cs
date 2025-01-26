using HumanResourceTask.Api.Authentication;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.Services;

namespace HumanResourceTask.Api.Endpoints.Employee
{
    public class GetEmployeeEndpoint : ResultHandlingEndpoint<GetEmployeeRequest, EmployeeResponse>
    {
        private readonly IEmployeeService _employeeService;

        public GetEmployeeEndpoint(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public override void Configure()
        {
            Get("/employee/{id}");
            Policies(PolicyNames.GetEmployee);
        }

        public override async Task HandleAsync(GetEmployeeRequest req, CancellationToken ct)
        {
            var result = await _employeeService.GetEmployeeAsync(req.Id);

            if (!result.IsSuccess)
            {
                await HandleFailureAsync(result, ct);
            }
            else
            {
                var employeeRecord = result.Value;

                var response = employeeRecord.ToResponseDto();

                await SendAsync(response, cancellation: ct);
            }
        }
    }
}
