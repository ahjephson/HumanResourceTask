namespace HumanResourceTask.Api.Dto.Status
{
    public record ListStatusesResponse
    {
        public ListStatusesResponse(IReadOnlyList<StatusListItem> items)
        {
            Items = items;
        }

        public IReadOnlyList<StatusListItem> Items { get; }
    }
}
