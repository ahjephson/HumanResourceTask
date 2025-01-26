using FastEndpoints;
using HumanResourceTask.Api.Dto.Status;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.Services;

namespace HumanResourceTask.Api.Endpoints.Statuses
{
    public class ListStatusesEndpoint : ResultHandlingEndpoint<EmptyRequest, ListStatusesResponse>
    {
        private readonly IStatusService _statusService;

        public ListStatusesEndpoint(IStatusService statusService)
        {
            _statusService = statusService;
        }

        public override void Configure()
        {
            Get("/statuses");
        }

        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            var result = await _statusService.GetStatusesAsync();

            if (!result.IsSuccess)
            {
                await HandleFailureAsync(result, ct);
            }
            else
            {
                var listItems = result.Value.ToListItemDtos().ToList();
                var response = new ListStatusesResponse(listItems);

                await SendAsync(response, cancellation: ct);
            }
        }
    }
}
