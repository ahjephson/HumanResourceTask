using FastEndpoints;
using FluentAssertions;
using FluentResults;
using HumanResourceTask.Api.Authentication;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Api.Endpoints.Employee;
using HumanResourceTask.Errors;
using HumanResourceTask.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HumanResourceTask.Api.Test.Endpoints.Employee
{
    public class DeleteEmployeeEndpointTests
    {
        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_DeleteEmployeeSucceeds_THEN_ShouldReturnNoContent()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(s => s.DeleteEmployeeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(Result.Ok());

            var endpoint = Factory.Create<DeleteEmployeeEndpoint>(employeeServiceMock.Object);

            await endpoint.HandleAsync(new DeleteEmployeeRequest
            {
                Id = Guid.NewGuid(),
            }, CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_DeleteEmployeeFails_THEN_ShouldInvokeHandleFailureAsync()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(s => s.DeleteEmployeeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(Result.Fail(new NotFoundError("Employee", Guid.NewGuid())));

            var endpoint = Factory.Create<DeleteEmployeeEndpoint>(employeeServiceMock.Object);

            await endpoint.HandleAsync(new DeleteEmployeeRequest
            {
                Id = Guid.NewGuid(),
            }, CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public void GIVEN_AnyState_WHEN_EndpointIsCreated_THEN_ShouldHaveCorrectConfiguration()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();

            var endpoint = Factory.Create<DeleteEmployeeEndpoint>(employeeServiceMock.Object);

            endpoint.Definition.Verbs.Should().ContainSingle().Which.Should().Be("DELETE");
            endpoint.Definition.Routes.Should().ContainSingle().Which.Should().Be("/employee/{id}");
            endpoint.Definition.PreBuiltUserPolicies.Should().ContainSingle().Which.Should().Be(PolicyNames.DeleteEmployee);
        }
    }
}
