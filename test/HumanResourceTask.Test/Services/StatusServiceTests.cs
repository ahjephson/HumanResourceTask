using FluentAssertions;
using HumanResourceTask.Models;
using HumanResourceTask.Repositories;
using HumanResourceTask.Services;
using Moq;

namespace HumanResourceTask.Test.Services
{
    public class StatusServiceTests
    {
        [Fact]
        public async Task GIVEN_StatusRepositoryReturnsStatuses_WHEN_GetStatusesAsyncIsCalled_THEN_ShouldReturnSuccessResultWithStatuses()
        {
            var mockStatusRepository = new Mock<IStatusRepository>();
            var statuses = new List<Status>
            {
                new Status { Id = Guid.NewGuid(), Name = "Name1" },
                new Status { Id = Guid.NewGuid(), Name = "Name2" }
            };

            mockStatusRepository
                .Setup(repo => repo.ListStatusesAsync())
                .ReturnsAsync(statuses);

            var service = new StatusService(mockStatusRepository.Object);

            var result = await service.GetStatusesAsync();

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(statuses);
        }

        [Fact]
        public async Task GIVEN_StatusRepositoryThrowsException_WHEN_GetStatusesAsyncIsCalled_THEN_ShouldReturnFailureResult()
        {
            var mockStatusRepository = new Mock<IStatusRepository>();

            mockStatusRepository
                .Setup(repo => repo.ListStatusesAsync())
                .ThrowsAsync(new Exception("Message"));

            var service = new StatusService(mockStatusRepository.Object);

            var result = await service.GetStatusesAsync();

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == "Message");
        }
    }
}
