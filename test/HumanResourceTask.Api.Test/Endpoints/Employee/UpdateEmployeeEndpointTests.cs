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
    public class UpdateEmployeeEndpointTests
    {
        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_UpdateEmployeeSucceeds_THEN_ShouldReturnUpdatedEmployeeResponse()
        {
            var employeeId = Guid.NewGuid();
            var updatedEmployee = new EmployeeRecordView
            {
                Id = employeeId,
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Email = "UpdatedEmail",
                DepartmentName = "UpdatedDepartment",
                StatusName = "UpdatedStatus"
            };

            var employeeRecord = new EmployeeRecordView
            {
                Id = employeeId,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345,
                DepartmentName = "OriginalDepartment",
                StatusName = "OriginalStatus"
            };

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(s => s.GetEmployeeAsync(employeeId))
                .ReturnsAsync(Result.Ok(employeeRecord));
            employeeServiceMock
                .Setup(s => s.UpdateEmployeeAsync(It.IsAny<EmployeeRecord>()))
                .ReturnsAsync(Result.Ok(updatedEmployee));

            var endpoint = Factory.Create<UpdateEmployeeEndpoint>(employeeServiceMock.Object);

            var request = new UpdateEmployeeRequest
            {
                Id = employeeId,
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Email = "UpdatedEmail"
            };

            await endpoint.HandleAsync(request, CancellationToken.None);

            endpoint.Response.Should().NotBeNull();
            endpoint.Response.Id.Should().Be(employeeId);
            endpoint.Response.FirstName.Should().Be("UpdatedFirstName");
            endpoint.Response.LastName.Should().Be("UpdatedLastName");
            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);

            employeeServiceMock.Verify(s =>
                s.UpdateEmployeeAsync(It.Is<EmployeeRecord>(er =>
                    er.Id == employeeId &&
                    er.FirstName == "UpdatedFirstName" &&
                    er.LastName == "UpdatedLastName" &&
                    er.Email == "UpdatedEmail")), Times.Once);
        }

        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_GetEmployeeFails_THEN_ShouldReturnNotFound()
        {
            var employeeId = Guid.NewGuid();

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(s => s.GetEmployeeAsync(employeeId))
                .ReturnsAsync(Result.Fail(new NotFoundError("Employee", employeeId)));

            var endpoint = Factory.Create<UpdateEmployeeEndpoint>(employeeServiceMock.Object);

            var request = new UpdateEmployeeRequest
            {
                Id = employeeId,
            };

            await endpoint.HandleAsync(request, CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            employeeServiceMock.Verify(s => s.UpdateEmployeeAsync(It.IsAny<EmployeeRecord>()), Times.Never);
        }

        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_UpdateEmployeeFails_THEN_ShouldReturnBadRequest()
        {
            var employeeId = Guid.NewGuid();

            var employeeRecord = new EmployeeRecordView
            {
                Id = employeeId,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345,
                DepartmentName = "OriginalDepartment",
                StatusName = "OriginalStatus"
            };

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(s => s.GetEmployeeAsync(employeeId))
                .ReturnsAsync(Result.Ok(employeeRecord));
            employeeServiceMock
                .Setup(s => s.UpdateEmployeeAsync(It.IsAny<EmployeeRecord>()))
                .ReturnsAsync(Result.Fail(new ValidationError("PropertyName", "Message")));

            var endpoint = Factory.Create<UpdateEmployeeEndpoint>(employeeServiceMock.Object);

            var request = new UpdateEmployeeRequest
            {
                Id = employeeId,
            };

            await endpoint.HandleAsync(request, CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            employeeServiceMock.Verify(s => s.UpdateEmployeeAsync(It.IsAny<EmployeeRecord>()), Times.Once);
        }

        [Fact]
        public void GIVEN_AnyState_WHEN_EndpointIsCreated_THEN_ShouldHaveCorrectConfiguration()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            var endpoint = Factory.Create<UpdateEmployeeEndpoint>(employeeServiceMock.Object);

            endpoint.Definition.Verbs.Should().ContainSingle().Which.Should().Be("PATCH");
            endpoint.Definition.Routes.Should().ContainSingle().Which.Should().Be("/employee/{id}");
            endpoint.Definition.PreBuiltUserPolicies.Should().ContainSingle().Which.Should().Be(PolicyNames.UpdateEmployee);
        }
    }
}
