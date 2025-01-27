using HumanResourceTask.Api.Authentication;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Api.Dto.Validation;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.Services;

namespace HumanResourceTask.Api.Endpoints.Employee
{
    public class CreateEmployeeEndpoint : ResultHandlingEndpoint<CreateEmployeeRequest, EmployeeResponse>
    {
        private readonly IEmployeeService _employeeService;

        public CreateEmployeeEndpoint(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public override void Configure()
        {
            Post("/employee");
            Policies(PolicyNames.CreateEmployee);
            Validator<CreateEmployeeRequestValidator>();
        }

        public override async Task HandleAsync(CreateEmployeeRequest req, CancellationToken ct)
        {
            var result = await _employeeService.CreateEmployeeAsync(
                req.FirstName,
                req.LastName,
                req.Email,
                req.DateOfBirth.Value,
                req.DepartmentId.Value,
                req.StatusId.Value,
                req.EmployeeNumber.Value);

            if (!result.IsSuccess)
            {
                await HandleFailureAsync(result, ct);
            }
            else
            {
                var createdEmployee = result.Value;

                var response = createdEmployee.ToResponseDto();

                await SendCreatedAtAsync<GetEmployeeEndpoint>(new
                {
                    id = createdEmployee.Id
                }, response, cancellation: ct);
            }
        }
    }
}
