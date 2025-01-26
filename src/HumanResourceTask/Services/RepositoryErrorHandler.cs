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
                if (repositoryException.ErrorType == RepositoryErrorType.ConstraintViolation)
                {
                    return new ValidationError(repositoryException.ColumnName, "Must be unique.");
                }
                if (repositoryException.ErrorType == RepositoryErrorType.ForeignKeyVoilation)
                {
                    return new ValidationError(repositoryException.ColumnName, "Has an invalid value.");
                }
                else if (repositoryException.ErrorType == RepositoryErrorType.SqlException)
                {
                    return new ExceptionalError(exception.InnerException);
                }
                else
                {
                    return new Error(repositoryException.Message);
                }
            }

            return new ExceptionalError(exception);
        }
    }
}
