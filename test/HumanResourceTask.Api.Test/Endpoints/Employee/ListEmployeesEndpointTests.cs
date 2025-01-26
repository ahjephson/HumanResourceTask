using FastEndpoints;
using FluentAssertions;
using FluentResults;
using HumanResourceTask.Api.Authentication;
using HumanResourceTask.Api.Dto;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Api.Endpoints.Employee;
using HumanResourceTask.MetaModels;
using HumanResourceTask.Models;
using HumanResourceTask.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HumanResourceTask.Api.Test.Endpoints.Employee
{
    public class ListEmployeesEndpointTests
    {
        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_ListEmployeesSucceeds_THEN_ShouldReturnEmployeeListResponse()
        {
            var employees = new List<EmployeeRecordView>
            {
                new EmployeeRecordView
                {
                    Id = Guid.NewGuid(),
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Email = "Email1",
                    DepartmentName = "Department1",
                    StatusName = "Status1"
                },
                new EmployeeRecordView
                {
                    Id = Guid.NewGuid(),
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Email = "Email2",
                    DepartmentName = "Department2",
                    StatusName = "Status2"
                }
            };

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(s => s.ListEmployeeRecordsAsync(It.IsAny<SortDefinition>(), It.IsAny<PaginationDefinition>(), It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ReturnsAsync(value: Result.Ok(new Paginated<EmployeeRecordView>(employees, 2, false)));

            var endpoint = Factory.Create<ListEmployeesEndpoint>(employeeServiceMock.Object);

            var request = new ListEmployeesRequest
            {
                Sort = new Sort { Column = "FirstName", Ascending = true },
                Limit = 10,
                Offset = 0
            };

            await endpoint.HandleAsync(request, CancellationToken.None);

            endpoint.Response.Should().NotBeNull();
            endpoint.Response.Items.Should().HaveCount(2);
            endpoint.Response.HasMore.Should().BeFalse();

            employeeServiceMock.Verify(s =>
                s.ListEmployeeRecordsAsync(
                    It.Is<SortDefinition>(sd => sd.ColumnName == "FirstName" && sd.Direction == SortDirection.Ascending),
                    It.Is<PaginationDefinition>(pd => pd.Limit == 10 && pd.Offset == 0),
                    It.IsAny<Guid?>(),
                    It.IsAny<Guid?>()),
                Times.Once);
        }

        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_ListEmployeesFails_THEN_ShouldHandleFailure()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(s => s.ListEmployeeRecordsAsync(It.IsAny<SortDefinition>(), It.IsAny<PaginationDefinition>(), It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ReturnsAsync(Result.Fail("Some error occurred"));

            var endpoint = Factory.Create<ListEmployeesEndpoint>(employeeServiceMock.Object);

            var request = new ListEmployeesRequest
            {
                Sort = new Sort { Column = "FirstName", Ascending = true },
                Limit = 10,
                Offset = 0
            };

            await endpoint.HandleAsync(request, CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            employeeServiceMock.Verify(s =>
                s.ListEmployeeRecordsAsync(
                    It.Is<SortDefinition>(sd => sd.ColumnName == "FirstName" && sd.Direction == SortDirection.Ascending),
                    It.Is<PaginationDefinition>(pd => pd.Limit == 10 && pd.Offset == 0),
                    It.IsAny<Guid?>(),
                    It.IsAny<Guid?>()),
                Times.Once);
        }

        [Fact]
        public void GIVEN_AnyState_WHEN_EndpointIsCreated_THEN_ShouldHaveCorrectConfiguration()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            var endpoint = Factory.Create<ListEmployeesEndpoint>(employeeServiceMock.Object);

            endpoint.Definition.Verbs.Should().ContainSingle().Which.Should().Be("GET");
            endpoint.Definition.Routes.Should().ContainSingle().Which.Should().Be("/employees");
            endpoint.Definition.PreBuiltUserPolicies.Should().ContainSingle().Which.Should().Be(PolicyNames.GetEmployee);
        }
    }
}
