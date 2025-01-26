using HumanResourceTask.Api.Dto.Department;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Api.Dto.Status;
using HumanResourceTask.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace HumanResourceTask.Web.Pages
{
    [Authorize]
    public partial class EmployeeList
    {
        [Inject]
        protected IApiClient ApiClient { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected IDialogService DialogService { get; set; } = default!;

        [Inject]
        protected ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        protected IOptions<ApplicationOptions> ApplicationOptions { get; set; } = default!;

        protected MudDataGrid<EmployeeListItem>? DataGrid { get; set; }

        public IEnumerable<DepartmentListItem> Departments { get; private set; } = [];
        public IEnumerable<StatusListItem> Statuses { get; private set; } = [];

        protected Guid? DepartmentId { get; set; }

        protected Guid? StatusId { get; set; }

        protected int PageSize => ApplicationOptions.Value.PageSize;

        protected override async Task OnInitializedAsync()
        {
            Departments = (await ApiClient.GetDepartmentsAsync()).Value;
            Statuses = (await ApiClient.GetStatusesAsync()).Value;
        }

        private async Task<GridData<EmployeeListItem>> ServerReload(GridState<EmployeeListItem> state)
        {
            var sortDefinition = state.SortDefinitions.FirstOrDefault();
            var request = new ListEmployeesRequest
            {
                Offset = state.Page * state.PageSize,
                Limit = state.PageSize,
                Sort = sortDefinition is null ? null : new Api.Dto.Sort { Ascending = !sortDefinition.Descending, Column = sortDefinition.SortBy },
                DepartmentId = DepartmentId,
                StatusId = StatusId,
            };

            var result = await ApiClient.ListEmployeesAsync(request);
            var data = result.Value;
            return new GridData<EmployeeListItem>
            {
                TotalItems = (int)data.Total,
                Items = data.Items
            };
        }

        private async Task ClearDepartmentFilter()
        {
            DepartmentId = null;

            if (DataGrid is not null)
            {
                await DataGrid.ReloadServerData();
            }
        }

        private async Task ApplyDepartmentFilter()
        {
            if (DataGrid is not null)
            {
                await DataGrid.ReloadServerData();
            }
        }

        private async Task ClearStatusFilter()
        {
            StatusId = null;

            if (DataGrid is not null)
            {
                await DataGrid.ReloadServerData();
            }
        }

        private async Task ApplyStatusFilter()
        {
            if (DataGrid is not null)
            {
                await DataGrid.ReloadServerData();
            }
        }

        private void Edit(CellContext<EmployeeListItem> cellContext)
        {
            NavigationManager.NavigateTo($"/employee/details/{cellContext.Item.Id}");
        }

        private async Task Delete(CellContext<EmployeeListItem> cellContext)
        {
            var delete = await DialogService.ShowMessageBox("Delete", "Are you sure you want to delete this employee?", cancelText: "Cancel");
            if (!delete.GetValueOrDefault())
            {
                return;
            }

            var result = await ApiClient.DeleteEmployeeAsync(cellContext.Item.Id);
            if (!result.IsSuccess)
            {
                Snackbar.Add("Unable to delete employee.", MudBlazor.Severity.Error);
            }
            else
            {
                Snackbar.Add("Employee deleted.", MudBlazor.Severity.Success);

                if (DataGrid is not null)
                {
                    await DataGrid.ReloadServerData();
                }
            }
        }
    }
}
