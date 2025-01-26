using FluentValidation;
using HumanResourceTask.Api.Dto.Department;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Api.Dto.Status;
using HumanResourceTask.Api.Dto.Validation;
using HumanResourceTask.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Humanizer;

namespace HumanResourceTask.Web.Pages
{
    [Authorize]
    public partial class EmployeeDetails
    {
        [Inject]
        protected IApiClient ApiClient { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = default!;

        [Parameter]
        public Guid Id { get; set; }

        public IEnumerable<DepartmentListItem> Departments { get; private set; } = [];
        public IEnumerable<StatusListItem> Statuses { get; private set; } = [];

        private MudForm Form { get; set; } = default!;

        private MudSelect<Guid?> DepartmentSelect { get; set; } = default!;
        private MudSelect<Guid?> StatusSelect { get; set; } = default!;

        private string? ServerError { get; set; }

        private UpdateEmployeeRequest Model { get; set; } = new UpdateEmployeeRequest { Id = Guid.Empty };

        private UpdateEmployeeRequestValidator Validator { get; } = new UpdateEmployeeRequestValidator();

        protected override async Task OnInitializedAsync()
        {
            Departments = (await ApiClient.GetDepartmentsAsync()).Value;
            Statuses = (await ApiClient.GetStatusesAsync()).Value;

            var result = await ApiClient.GetEmployeeAsync(Id);
            var employee = result.Value;

            Model = new UpdateEmployeeRequest
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DateOfBirth = employee.DateOfBirth,
                DepartmentId = employee.DepartmentId,
                StatusId = employee.StatusId,
                EmployeeNumber = employee.EmployeeNumber
            };

            DepartmentSelect?.ForceRender(true);
            StatusSelect?.ForceRender(true);
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await Validator.ValidateAsync(ValidationContext<UpdateEmployeeRequest>.CreateWithOptions((UpdateEmployeeRequest)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
            {
                return [];
            }

            return result.Errors.Select(e => e.ErrorMessage);
        };

        private async Task Submit()
        {
            await Form.Validate();

            if (!Form.IsValid)
            {
                return;
            }

            var result = await ApiClient.UpdateEmployeeAsync(Model);
            if (!result.IsSuccess)
            {
                if (result.HasError<ErrorResponse>(out var errors))
                {
                    ServerError = ApiClient.GetFriendlyError(errors.First());
                }
            }
            else
            {
                Snackbar.Add("Employee updated.", MudBlazor.Severity.Success);
            }
        }
    }
}
