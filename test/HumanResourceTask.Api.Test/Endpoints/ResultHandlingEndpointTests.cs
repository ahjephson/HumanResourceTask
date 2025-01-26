using FastEndpoints;
using FluentAssertions;
using FluentResults;
using HumanResourceTask.Api.Endpoints;
using HumanResourceTask.Errors;
using Microsoft.AspNetCore.Http;

namespace HumanResourceTask.Api.Test.Endpoints
{
    public class ResultHandlingEndpointTests
    {
        public class ConcreteResultHandlingEndpoint : ResultHandlingEndpoint<RequestObject>
        {
            public override async Task HandleAsync(RequestObject req, CancellationToken ct)
            {
                if (req.TestResult.IsSuccess)
                {
                    throw new InvalidOperationException("This should only test failures.");
                }
                await HandleFailureAsync(req.TestResult, ct);
            }
        }

        public class ConcreteResultHandlingEndpointWithResponse : ResultHandlingEndpoint<RequestObject, ResponseObject>
        {
            public override async Task HandleAsync(RequestObject req, CancellationToken ct)
            {
                if (req.TestResult.IsSuccess)
                {
                    throw new InvalidOperationException("This should only test failures.");
                }
                await HandleFailureAsync(req.TestResult, ct);
            }
        }

        public class RequestObject
        {
            public RequestObject(Result testResult)
            {
                TestResult = testResult;
            }

            public Result TestResult { get; }
        }

        public class ResponseObject
        {
            // marker
        }

        [Fact]
        public async Task GIVEN_NotFoundError_WHEN_HandleFailureAsync_THEN_ShouldSendNotFound()
        {
            var endpoint = Factory.Create<ConcreteResultHandlingEndpoint>();
            var result = Result.Fail(new NotFoundError("User", Guid.NewGuid()));

            await endpoint.HandleAsync(new RequestObject(result), CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GIVEN_NotFoundError_WithResponse_WHEN_HandleFailureAsync_THEN_ShouldSendNotFound()
        {
            var endpoint = Factory.Create<ConcreteResultHandlingEndpointWithResponse>();
            var result = Result.Fail(new NotFoundError("User", Guid.NewGuid()));

            await endpoint.HandleAsync(new RequestObject(result), CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GIVEN_ValidationError_WHEN_HandleFailureAsync_THEN_ShouldSendBadRequest()
        {
            var endpoint = Factory.Create<ConcreteResultHandlingEndpoint>();
            var validationError = new ValidationError("Name", "Name is required.");
            var result = Result.Fail(validationError);

            await endpoint.HandleAsync(new RequestObject(result), CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            endpoint.ValidationFailures.Should().ContainSingle(f =>
                f.PropertyName == validationError.PropertyName &&
                f.ErrorMessage == validationError.Message);
        }

        [Fact]
        public async Task GIVEN_ValidationError_WithResponse_WHEN_HandleFailureAsync_THEN_ShouldSendBadRequest()
        {
            var endpoint = Factory.Create<ConcreteResultHandlingEndpointWithResponse>();
            var validationError = new ValidationError("Email", "Email is invalid.");
            var result = Result.Fail(validationError);

            await endpoint.HandleAsync(new RequestObject(result), CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            endpoint.ValidationFailures.Should().ContainSingle(f =>
                f.PropertyName == validationError.PropertyName &&
                f.ErrorMessage == validationError.Message);
        }

        [Fact]
        public async Task GIVEN_ExceptionalError_WHEN_HandleFailureAsync_THEN_ShouldSendInternalServerError()
        {
            var endpoint = Factory.Create<ConcreteResultHandlingEndpoint>();
            var exceptionalError = new ExceptionalError(new Exception("Something went wrong"));
            var result = Result.Fail(exceptionalError);

            await endpoint.HandleAsync(new RequestObject(result), CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task GIVEN_ExceptionalError_WithResponse_WHEN_HandleFailureAsync_THEN_ShouldSendInternalServerError()
        {
            var endpoint = Factory.Create<ConcreteResultHandlingEndpointWithResponse>();
            var exceptionalError = new ExceptionalError(new Exception("Unexpected failure"));
            var result = Result.Fail(exceptionalError);

            await endpoint.HandleAsync(new RequestObject(result), CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task GIVEN_UnknownError_WHEN_HandleFailureAsync_THEN_ShouldSendInternalServerError()
        {
            var endpoint = Factory.Create<ConcreteResultHandlingEndpoint>();
            var result = Result.Fail("An unknown error occurred.");

            await endpoint.HandleAsync(new RequestObject(result), CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task GIVEN_UnknownError_WithResponse_WHEN_HandleFailureAsync_THEN_ShouldSendInternalServerError()
        {
            var endpoint = Factory.Create<ConcreteResultHandlingEndpointWithResponse>();
            var result = Result.Fail("Unknown error encountered.");

            await endpoint.HandleAsync(new RequestObject(result), CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
