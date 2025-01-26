using FluentAssertions;
using HumanResourceTask.Api.Dto.Status;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.Models;

namespace HumanResourceTask.Api.Test.Mapping
{
    public class StatusMappingTests
    {
        [Fact]
        public void GIVEN_Status_WHEN_ToListItemDtoIsCalled_THEN_ShouldMapCorrectly()
        {
            var status = new Status
            {
                Id = Guid.NewGuid(),
                Name = "Name"
            };

            var result = status.ToListItemDto();

            result.Id.Should().Be(status.Id);
            result.Name.Should().Be(status.Name);
        }

        [Fact]
        public void GIVEN_StatusCollection_WHEN_ToListItemDtosIsCalled_THEN_ShouldMapAllStatusesCorrectly()
        {
            var statuses = new List<Status>
            {
                new Status
                {
                    Id = Guid.NewGuid(),
                    Name = "Name1"
                },
                new Status
                {
                    Id = Guid.NewGuid(),
                    Name = "Name2"
                }
            };

            var result = statuses.ToListItemDtos();

            result.Should().HaveCount(2);
            result.Should().ContainEquivalentOf(new StatusListItem
            {
                Id = statuses[0].Id,
                Name = statuses[0].Name
            });
            result.Should().ContainEquivalentOf(new StatusListItem
            {
                Id = statuses[1].Id,
                Name = statuses[1].Name
            });
        }
    }
}
