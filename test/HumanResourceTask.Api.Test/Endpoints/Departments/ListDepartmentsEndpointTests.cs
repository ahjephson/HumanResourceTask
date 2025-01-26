using FastEndpoints;
using FluentAssertions;
using FluentResults;
using HumanResourceTask.Api.Endpoints.Departments;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.Models;
using HumanResourceTask.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HumanResourceTask.Api.Test.Endpoints.Departments
{
    public class ListDepartmentsEndpointTests
    {
        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_GetDepartmentsSucceeds_THEN_ShouldReturnOkWithDepartmentList()
        {
            var departmentServiceMock = new Mock<IDepartmentService>();
            var departments = new List<Department>
            {
                new Department { Id = Guid.NewGuid(), Name = "Name1" },
                new Department { Id = Guid.NewGuid(), Name = "Name2" }
            };
            departmentServiceMock
                .Setup(s => s.GetDepartmentsAsync())
                .ReturnsAsync(Result.Ok<IReadOnlyList<Department>>(departments));

            var endpoint = Factory.Create<ListDepartmentsEndpoint>(departmentServiceMock.Object);

            await endpoint.HandleAsync(new EmptyRequest(), CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
            var response = endpoint.Response;
            response.Should().NotBeNull();
            response.Items.Should().BeEquivalentTo(departments.ToListItemDtos());
        }

        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_GetDepartmentsFails_THEN_ShouldInvokeHandleFailureAsync()
        {
            var departmentServiceMock = new Mock<IDepartmentService>();
            departmentServiceMock
                .Setup(s => s.GetDepartmentsAsync())
                .ReturnsAsync(Result.Fail("Error"));

            var endpoint = Factory.Create<ListDepartmentsEndpoint>(departmentServiceMock.Object);

            await endpoint.HandleAsync(new EmptyRequest(), CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().NotBe(StatusCodes.Status200OK);
        }

        [Fact]
        public void GIVEN_AnyState_WHEN_EndpointIsCreated_THEN_ShouldHaveCorrectConfiguration()
        {
            var departmentServiceMock = new Mock<IDepartmentService>();

            var endpoint = Factory.Create<ListDepartmentsEndpoint>(departmentServiceMock.Object);

            endpoint.Definition.Verbs.Should().ContainSingle().Which.Should().Be("GET");
            endpoint.Definition.Routes.Should().ContainSingle().Which.Should().Be("/departments");
        }
    }
}
