using FluentAssertions;
using HumanResourceTask.Exceptions;
using Microsoft.Data.SqlClient;

namespace HumanResourceTask.Data.Test
{
    public class RepositoryExceptionFactoryTests
    {
        [Fact]
        public void GIVEN_SqlExceptionForUniqueConstraint_WHEN_FromSqlException_THEN_ShouldReturnRepositoryException()
        {
            var sqlException = CreateSqlException(
                2601,
                "Cannot insert duplicate key row in object 'dbo.Table' with unique index 'UQ_Table_Column'. The duplicate key value is (value).");

            var exception = RepositoryExceptionFactory.FromSqlException(sqlException);

            exception.Should().NotBeNull();
            exception.ErrorType.Should().Be(RepositoryErrorType.ConstraintViolation);
            exception.ColumnName.Should().Be("Column");
            exception.DuplicateKeyValue.Should().Be("value");
            exception.InnerException.Should().Be(sqlException);
        }

        [Fact]
        public void GIVEN_SqlExceptionForForeignKeyViolation_WHEN_FromSqlException_THEN_ShouldReturnRepositoryException()
        {
            var sqlException = CreateSqlException(
                547,
                @"The INSERT statement conflicted with the FOREIGN KEY constraint ""FK_Table_Column"". The conflict occurred in database ""Database"", table ""dbo.Table"", column 'ColumnName'.");

            var exception = RepositoryExceptionFactory.FromSqlException(sqlException);

            exception.Should().NotBeNull();
            exception.ErrorType.Should().Be(RepositoryErrorType.ForeignKeyVoilation);
            exception.ColumnName.Should().Be("Column");
            exception.InnerException.Should().Be(sqlException);
        }

        [Fact]
        public void GIVEN_SqlExceptionForUnknownError_WHEN_FromSqlException_THEN_ShouldReturnSqlException()
        {
            var sqlException = CreateSqlException(9999, "An unknown database error occurred.");

            var exception = RepositoryExceptionFactory.FromSqlException(sqlException);

            exception.Should().NotBeNull();
            exception.ErrorType.Should().Be(RepositoryErrorType.SqlException);
            exception.ColumnName.Should().BeNull();
            exception.InnerException.Should().Be(sqlException);
        }

        private static SqlException CreateSqlException(int number, string message)
        {
            var builder = new SqlExceptionBuilder()
                .WithErrorNumber(number)
                .WithErrorMessage(message);

            return builder.Build();
        }
    }
}
