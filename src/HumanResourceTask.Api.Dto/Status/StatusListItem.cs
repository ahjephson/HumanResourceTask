namespace HumanResourceTask.Api.Dto.Status
{
    public record StatusListItem
    {
        public Guid Id { get; init; }

        public required string Name { get; set; }
    }
}
