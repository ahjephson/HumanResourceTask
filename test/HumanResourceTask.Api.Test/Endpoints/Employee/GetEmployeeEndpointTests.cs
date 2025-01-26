using FastEndpoints;
using FluentAssertions;
using FluentResults;
using HumanResourceTask.Api.Authentication;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Api.Endpoints.Employee;
using HumanResourceTask.Errors;
using HumanResourceTask.Models;
using HumanResourceTask.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HumanResourceTask.Api.Test.Endpoints.Employee
{
    public class GetEmployeeEndpointTests
    {
        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_GetEmployeeSucceeds_THEN_ShouldSetResponseAndReturnOk()
        {
            var employeeId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();
            var statusId = Guid.NewGuid();
            var employeeRecordView = new EmployeeRecordView
            {
                Id = employeeId,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1985, 1, 1),
                DepartmentId = departmentId,
                StatusId = statusId,
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = null,
                Deleted = false,
                DepartmentName = "DepartmentName",
                StatusName = "StatusName"
            };

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(s => s.GetEmployeeAsync(employeeId))
                .ReturnsAsync(Result.Ok(employeeRecordView));

            var endpoint = Factory.Create<GetEmployeeEndpoint>(employeeServiceMock.Object);

            var request = new GetEmployeeRequest { Id = employeeId };

            await endpoint.HandleAsync(request, CancellationToken.None);

            endpoint.Response.Should().NotBeNull();
            endpoint.Response.Id.Should().Be(employeeId);
            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_GetEmployeeFailsWithNotFoundError_THEN_ShouldReturnNotFound()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(s => s.GetEmployeeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(Result.Fail(new NotFoundError("Employee", Guid.NewGuid())));

            var endpoint = Factory.Create<GetEmployeeEndpoint>(employeeServiceMock.Object);

            var request = new GetEmployeeRequest { Id = Guid.NewGuid() };

            await endpoint.HandleAsync(request, CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public void GIVEN_AnyState_WHEN_ConfigureIsCalled_THEN_ShouldSetCorrectRouteAndPolicies()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();

            var endpoint = Factory.Create<GetEmployeeEndpoint>(employeeServiceMock.Object);

            endpoint.Definition.Verbs.Should().ContainSingle().Which.Should().Be("GET");
            endpoint.Definition.Routes.Should().ContainSingle().Which.Should().Be("/employee/{id}");
            endpoint.Definition.PreBuiltUserPolicies.Should().ContainSingle().Which.Should().Be(PolicyNames.GetEmployee);
        }
    }
}
