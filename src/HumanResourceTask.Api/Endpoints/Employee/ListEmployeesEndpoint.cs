using HumanResourceTask.Api.Authentication;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.MetaModels;
using HumanResourceTask.Services;

namespace HumanResourceTask.Api.Endpoints.Employee
{
    public class ListEmployeesEndpoint : ResultHandlingEndpoint<ListEmployeesRequest, ListEmployeesResponse>
    {
        private readonly IEmployeeService _employeeService;

        public ListEmployeesEndpoint(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public override void Configure()
        {
            Get("/employees");
            Policies(PolicyNames.GetEmployee);
        }

        public override async Task HandleAsync(ListEmployeesRequest req, CancellationToken ct)
        {
            var sort = req.Sort.ToModel();
            var pagination = PaginationDefinition.Defaults;
            if (req.Offset is not null)
            {
                pagination.Offset = req.Offset.Value;
            }
            if (req.Limit is not null)
            {
                pagination.Limit = req.Limit.Value;
            }

            var result = await _employeeService.ListEmployeeRecordsAsync(sort, pagination, req.StatusId, req.DepartmentId);

            if (!result.IsSuccess)
            {
                await HandleFailureAsync(result, ct);
            }
            else
            {
                var listItems = result.Value.Items.ToListItemDtos().ToList();
                var response = new ListEmployeesResponse
                {
                    Items = listItems,
                    Total = result.Value.Total,
                    HasMore = result.Value.HasMore,
                };

                await SendAsync(response, cancellation: ct);
            }
        }
    }
}
