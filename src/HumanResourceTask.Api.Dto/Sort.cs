namespace HumanResourceTask.Api.Dto
{
    public record Sort
    {
        public required string Column { get; init; }

        public bool Ascending { get; init; }
    }
}
