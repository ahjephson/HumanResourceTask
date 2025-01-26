using System.Text.RegularExpressions;
using HumanResourceTask.Exceptions;
using Microsoft.Data.SqlClient;

namespace HumanResourceTask.Data
{
    public static class RepositoryExceptionFactory
    {
        private static readonly Regex _uniqueIndexRegex = new Regex(@"UQ(?:__|_)([a-zA-Z0-9]+)_([a-zA-Z0-9]+)(?:_(\w+))?.*?The duplicate key value is \((.*?)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex _foreignKeyRegex = new Regex(@"FOREIGN KEY constraint \""FK_[^_]+_([^""]+)""", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static RepositoryException FromSqlException(SqlException sqlException)
        {
            switch (sqlException.Number)
            {
                case 2601: // Unique index or primary key violation
                case 2627: // Unique constraint violation
                    {
                        string? columnName = null;
                        string? duplicateKeyValue = null;
                        var match = _uniqueIndexRegex.Match(sqlException.Message);
                        if (match.Success)
                        {
                            columnName = match.Groups[2].Value;
                            duplicateKeyValue = match.Groups[4].Value;
                        }
                        return new RepositoryException(
                            "A constraint violation occurred.",
                            RepositoryErrorType.ConstraintViolation,
                            columnName,
                            duplicateKeyValue,
                            sqlException
                        );
                    }
                case 547: // Foreign key violation
                    {
                        string? columnName = null;
                        var match = _foreignKeyRegex.Match(sqlException.Message);
                        if (match.Success)
                        {
                            columnName = match.Groups[1].Value;
                        }
                        return new RepositoryException(
                            "A foreign key constraint violation occurred.",
                            RepositoryErrorType.ForeignKeyVoilation,
                            columnName,
                            null,
                            sqlException
                        );
                    }

                default:
                    return new RepositoryException(
                        "A database error occurred.",
                        RepositoryErrorType.SqlException,
                        innerException: sqlException
                    );
            }
        }
    }
}
