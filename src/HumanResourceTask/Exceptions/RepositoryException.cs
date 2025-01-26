namespace HumanResourceTask.Exceptions
{
    public class RepositoryException : Exception
    {
        public RepositoryErrorType ErrorType { get; }

        public string? ColumnName { get; }

        public string? DuplicateKeyValue { get; }

        public RepositoryException(
            string message,
            RepositoryErrorType errorType = RepositoryErrorType.Unknown,
            string? columnName = null,
            string? duplicateKeyValue = null,

            Exception? innerException = null)
            : base(message, innerException)
        {
            ErrorType = errorType;
            ColumnName = columnName;
            DuplicateKeyValue = duplicateKeyValue;
        }
    }
}
