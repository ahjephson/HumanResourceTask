using FastEndpoints;
using HumanResourceTask.Api.Dto.Department;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.Services;

namespace HumanResourceTask.Api.Endpoints.Departments
{
    public class ListDepartmentsEndpoint : ResultHandlingEndpoint<EmptyRequest, ListDepartmentsResponse>
    {
        private readonly IDepartmentService _departmentService;

        public ListDepartmentsEndpoint(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public override void Configure()
        {
            Get("/departments");
        }

        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            var result = await _departmentService.GetDepartmentsAsync();

            if (!result.IsSuccess)
            {
                await HandleFailureAsync(result, ct);
            }
            else
            {
                var listItems = result.Value.ToListItemDtos().ToList();
                var response = new ListDepartmentsResponse(listItems);

                await SendAsync(response, cancellation: ct);
            }
        }
    }
}
