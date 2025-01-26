using FluentResults;
using HumanResourceTask.Errors;
using HumanResourceTask.Exceptions;

namespace HumanResourceTask.Services
{
    internal static class RepositoryErrorHandler
    {
        internal static IError HandleError(Exception exception)
        {
            if (exception is RepositoryException repositoryException)
            {
                return repositoryException.ErrorType switch
                {
                    RepositoryErrorType.ConstraintViolation => new ValidationError(repositoryException.ColumnName, "Must be unique."),
                    RepositoryErrorType.ForeignKeyVoilation => new ValidationError(repositoryException.ColumnName, "Has an invalid value."),
                    RepositoryErrorType.SqlException => new ExceptionalError(exception.InnerException),
                    _ => new Error(repositoryException.Message),
                };
            }

            return new ExceptionalError(exception);
        }
    }
}
