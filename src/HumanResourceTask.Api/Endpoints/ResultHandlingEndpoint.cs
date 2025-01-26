using FluentResults;
using FluentValidation.Results;
using HumanResourceTask.Errors;

namespace HumanResourceTask.Api.Endpoints
{
    public abstract class ResultHandlingEndpoint<TRequest> : FastEndpoints.Endpoint<TRequest> where TRequest : notnull
    {
        protected async Task HandleFailureAsync(ResultBase result, CancellationToken cancellationToken)
        {
            if (result.HasError<NotFoundError>())
            {
                await SendNotFoundAsync(cancellationToken);
            }
            else if (result.HasError<ValidationError>(out var validationErrors))
            {
                foreach (var error in validationErrors)
                {
                    ValidationFailures.Add(new ValidationFailure(error.PropertyName, error.Message));
                }

                await SendErrorsAsync(StatusCodes.Status400BadRequest, cancellationToken);
            }
            else if (result.HasError<ExceptionalError>(out var exceptionalErrors))
            {
                foreach (var error in exceptionalErrors)
                {
                    Logger.LogError(error.Exception, "An unhandled error occurred: {Message}.", error.Message);
                }
                await SendErrorsAsync(StatusCodes.Status500InternalServerError, cancellationToken);
            }
            else
            {
                await SendErrorsAsync(StatusCodes.Status500InternalServerError, cancellationToken);
            }
        }
    }

    public abstract class ResultHandlingEndpoint<TRequest, TResponse> : FastEndpoints.Endpoint<TRequest, TResponse> where TRequest : notnull
    {
        protected async Task HandleFailureAsync(ResultBase result, CancellationToken cancellationToken)
        {
            if (result.HasError<NotFoundError>())
            {
                await SendNotFoundAsync(cancellationToken);
            }
            else if (result.HasError<ValidationError>(out var validationErrors))
            {
                foreach (var error in validationErrors)
                {
                    ValidationFailures.Add(new ValidationFailure(error.PropertyName, error.Message));
                }

                await SendErrorsAsync(StatusCodes.Status400BadRequest, cancellationToken);
            }
            else if (result.HasError<ExceptionalError>(out var exceptionalErrors))
            {
                foreach (var error in exceptionalErrors)
                {
                    Logger.LogError(error.Exception, "An unhandled error occurred: {Message}.", error.Message);
                }
                await SendErrorsAsync(StatusCodes.Status500InternalServerError, cancellationToken);
            }
            else
            {
                await SendErrorsAsync(StatusCodes.Status500InternalServerError, cancellationToken);
            }
        }
    }
}
