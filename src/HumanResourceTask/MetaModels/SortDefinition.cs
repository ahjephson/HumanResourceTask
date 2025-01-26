namespace HumanResourceTask.MetaModels
{
    public class SortDefinition
    {
        public static readonly SortDefinition Defaults = new() { ColumnName = "Id", Direction = SortDirection.Ascending };

        public required string ColumnName { get; init; }

        public SortDirection Direction { get; set; }

        private string GetDirectionString()
        {
            return Direction == SortDirection.Ascending ? "ASC" : "DESC";
        }

        /// <summary>
        /// Returns a string that represents the formatted <seealso cref="SortDefinition"/>.
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return $"{ColumnName} {GetDirectionString()}";
        }
    }
}
