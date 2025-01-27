using FluentValidation;
using HumanResourceTask.Api.Dto.Department;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Api.Dto.Status;
using HumanResourceTask.Api.Dto.Validation;
using HumanResourceTask.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HumanResourceTask.Web.Pages
{
    [Authorize]
    public partial class EmployeeCreate
    {
        [Inject]
        protected IApiClient ApiClient { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = default!;

        public IEnumerable<DepartmentListItem> Departments { get; private set; } = [];

        public IEnumerable<StatusListItem> Statuses { get; private set; } = [];

        private MudForm Form { get; set; } = default!;

        private MudSelect<Guid?> DepartmentSelect { get; set; } = default!;

        private MudSelect<Guid?> StatusSelect { get; set; } = default!;

        private string? ServerError { get; set; }

        private CreateEmployeeRequest Model { get; set; } = new CreateEmployeeRequest();

        private CreateEmployeeRequestValidator Validator { get; } = new CreateEmployeeRequestValidator();

        protected override async Task OnInitializedAsync()
        {
            Departments = (await ApiClient.GetDepartmentsAsync()).Value;
            Statuses = (await ApiClient.GetStatusesAsync()).Value;

            Model = new CreateEmployeeRequest
            {
                FirstName = "",
                LastName = "",
                Email = ""
            };

            DepartmentSelect?.ForceRender(true);
            StatusSelect?.ForceRender(true);
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await Validator.ValidateAsync(ValidationContext<CreateEmployeeRequest>.CreateWithOptions((CreateEmployeeRequest)model, x => x.IncludeProperties(propertyName)));
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

            var result = await ApiClient.CreateEmployeeAsync(Model);
            if (!result.IsSuccess)
            {
                if (result.HasError<ErrorResponse>(out var errors))
                {
                    ServerError = ApiClient.GetFriendlyError(errors.First());
                }
            }
            else
            {
                Snackbar.Add("Employee created.", MudBlazor.Severity.Success);
                NavigationManager.NavigateTo($"/employee/details/{result.Value.Id}");
            }
        }
    }
}
