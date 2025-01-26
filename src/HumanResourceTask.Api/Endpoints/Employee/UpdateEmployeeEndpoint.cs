using HumanResourceTask.Api.Authentication;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Api.Dto.Validation;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.Services;

namespace HumanResourceTask.Api.Endpoints.Employee
{
    public class UpdateEmployeeEndpoint : ResultHandlingEndpoint<UpdateEmployeeRequest, EmployeeResponse>
    {
        private readonly IEmployeeService _employeeService;

        public UpdateEmployeeEndpoint(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public override void Configure()
        {
            Patch("/employee/{id}");
            Policies(PolicyNames.UpdateEmployee);
            Validator<UpdateEmployeeRequestValidator>();
        }

        public override async Task HandleAsync(UpdateEmployeeRequest req, CancellationToken ct)
        {
            var getResult = await _employeeService.GetEmployeeAsync(req.Id);
            if (!getResult.IsSuccess)
            {
                await HandleFailureAsync(getResult, ct);

                return;
            }

            var employeeRecord = getResult.Value;
            var updatedRecord = req.ToModel(employeeRecord);

            var updateResult = await _employeeService.UpdateEmployeeAsync(updatedRecord);

            if (!updateResult.IsSuccess)
            {
                await HandleFailureAsync(updateResult, ct);
            }
            else
            {
                var updatedEmployee = updateResult.Value;

                var response = updatedEmployee.ToResponseDto();

                await SendAsync(response, cancellation: ct);
            }
        }
    }
}
