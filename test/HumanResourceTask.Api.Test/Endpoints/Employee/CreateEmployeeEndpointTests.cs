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
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HumanResourceTask.Api.Test.Endpoints.Employee
{
    public class CreateEmployeeEndpointTests
    {
        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_CreateEmployeeSucceeds_THEN_ShouldSetResponseAndReturnCreated()
        {
            var employeeId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();
            var statusId = Guid.NewGuid();

            var createdEmployee = new EmployeeRecordView
            {
                Id = employeeId,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = departmentId,
                StatusId = statusId,
                EmployeeNumber = 12345,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = null,
                Deleted = false,
                DepartmentName = "DepartmentName",
                StatusName = "StatusName"
            };

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(s => s.CreateEmployeeAsync(
                    "FirstName",
                    "LastName",
                    "Email",
                    new DateOnly(1990, 1, 1),
                    departmentId,
                    statusId,
                    12345))
                .ReturnsAsync(Result.Ok(createdEmployee));

            var linkGeneratorMock = new Mock<LinkGenerator>();
            linkGeneratorMock
                .Setup(lg => lg.GetPathByAddress(
                    It.IsAny<string>(),
                    It.IsAny<RouteValueDictionary>(),
                    It.IsAny<PathString>(),
                    It.IsAny<FragmentString>(),
                    It.IsAny<LinkOptions?>()))
                .Returns($"/employee/{employeeId}");

            var endpoint = Factory.Create<CreateEmployeeEndpoint>(ctx =>
            {
                ctx.AddTestServices(services =>
                {
                    services.AddSingleton(employeeServiceMock.Object);
                    services.AddSingleton(linkGeneratorMock.Object);
                });
            });

            var request = new CreateEmployeeRequest
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = departmentId,
                StatusId = statusId,
                EmployeeNumber = 12345
            };

            await endpoint.HandleAsync(request, CancellationToken.None);

            endpoint.Response.Should().NotBeNull();
            endpoint.Response.Id.Should().Be(employeeId);
            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status201Created);

            var locationHeader = endpoint.HttpContext.Response.Headers.Location;
            locationHeader.Should().NotBeEmpty();
            locationHeader.ToString().Should().Be($"/employee/{employeeId}");
        }

        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_CreateEmployeeFailsWithValidationError_THEN_ShouldReturnBadRequest()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(s => s.CreateEmployeeAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<long>()))
                .ReturnsAsync(Result.Fail(new ValidationError("FirstName", "First name is required.")));

            var endpoint = Factory.Create<CreateEmployeeEndpoint>(employeeServiceMock.Object);

            var request = new CreateEmployeeRequest
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345
            };

            await endpoint.HandleAsync(request, CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_CreateEmployeeFailsWithNotFoundError_THEN_ShouldReturnNotFound()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(s => s.CreateEmployeeAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<long>()))
                .ReturnsAsync(Result.Fail(new NotFoundError("Department", Guid.NewGuid())));

            var endpoint = Factory.Create<CreateEmployeeEndpoint>(employeeServiceMock.Object);

            var request = new CreateEmployeeRequest
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345
            };

            await endpoint.HandleAsync(request, CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public void GIVEN_AnyState_WHEN_ConfigureIsCalled_THEN_ShouldSetCorrectRouteAndPolicies()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();

            var endpoint = Factory.Create<CreateEmployeeEndpoint>(employeeServiceMock.Object);

            endpoint.Definition.Verbs.Should().ContainSingle().Which.Should().Be("POST");
            endpoint.Definition.Routes.Should().ContainSingle().Which.Should().Be("/employee");
            endpoint.Definition.PreBuiltUserPolicies.Should().ContainSingle().Which.Should().Be(PolicyNames.CreateEmployee);
        }
    }
}
