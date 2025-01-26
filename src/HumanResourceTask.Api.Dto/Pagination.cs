namespace HumanResourceTask.Api.Dto
{
    public record Pagination
    {
        public int Offset { get; init; }

        public int Limit { get; init; }
    }
}
