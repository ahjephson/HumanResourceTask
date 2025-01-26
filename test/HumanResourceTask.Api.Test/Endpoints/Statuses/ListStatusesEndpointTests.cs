using FastEndpoints;
using FluentAssertions;
using FluentResults;
using HumanResourceTask.Api.Endpoints.Statuses;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.Models;
using HumanResourceTask.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace HumanResourceTask.Api.Test.Endpoints.Statuses
{
    public class ListStatusesEndpointTests
    {
        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_GetStatusesSucceeds_THEN_ShouldReturnOkWithStatusList()
        {
            var statusServiceMock = new Mock<IStatusService>();
            var statuses = new List<Status>
            {
                new Status { Id = Guid.NewGuid(), Name = "Name1" },
                new Status { Id = Guid.NewGuid(), Name = "Name2" }
            };
            statusServiceMock
                .Setup(s => s.GetStatusesAsync())
                .ReturnsAsync(Result.Ok<IReadOnlyList<Status>>(statuses));

            var endpoint = Factory.Create<ListStatusesEndpoint>(statusServiceMock.Object);

            await endpoint.HandleAsync(new EmptyRequest(), CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
            var response = endpoint.Response;
            response.Should().NotBeNull();
            response.Items.Should().BeEquivalentTo(statuses.ToListItemDtos());
        }

        [Fact]
        public async Task GIVEN_ValidRequest_WHEN_GetStatusesFails_THEN_ShouldInvokeHandleFailureAsync()
        {
            var statusServiceMock = new Mock<IStatusService>();
            statusServiceMock
                .Setup(s => s.GetStatusesAsync())
                .ReturnsAsync(Result.Fail("Error"));

            var endpoint = Factory.Create<ListStatusesEndpoint>(statusServiceMock.Object);

            await endpoint.HandleAsync(new EmptyRequest(), CancellationToken.None);

            endpoint.HttpContext.Response.StatusCode.Should().NotBe(StatusCodes.Status200OK);
        }

        [Fact]
        public void GIVEN_AnyState_WHEN_EndpointIsCreated_THEN_ShouldHaveCorrectConfiguration()
        {
            var statusServiceMock = new Mock<IStatusService>();

            var endpoint = Factory.Create<ListStatusesEndpoint>(statusServiceMock.Object);

            endpoint.Definition.Verbs.Should().ContainSingle().Which.Should().Be("GET");
            endpoint.Definition.Routes.Should().ContainSingle().Which.Should().Be("/statuses");
        }
    }
}
