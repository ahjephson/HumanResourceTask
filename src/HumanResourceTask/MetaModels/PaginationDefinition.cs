namespace HumanResourceTask.MetaModels
{
    public class PaginationDefinition
    {
        public static readonly PaginationDefinition Defaults = new() { Offset = 0, Limit = 20 };

        public int Offset { get; set; }

        public int Limit { get; set; }
    }
}
