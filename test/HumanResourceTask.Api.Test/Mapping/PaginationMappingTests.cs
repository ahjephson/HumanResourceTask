using FluentAssertions;
using HumanResourceTask.Api.Dto;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.MetaModels;

namespace HumanResourceTask.Api.Test.Mapping
{
    public class PaginationMappingTests
    {
        [Fact]
        public void GIVEN_NullPagination_WHEN_ToModelIsCalled_THEN_ShouldReturnDefaultPaginationDefinition()
        {
            Pagination? pagination = null;

            var result = pagination.ToModel();

            result.Should().BeEquivalentTo(PaginationDefinition.Defaults);
        }

        [Fact]
        public void GIVEN_ValidPagination_WHEN_ToModelIsCalled_THEN_ShouldMapToPaginationDefinition()
        {
            var pagination = new Pagination
            {
                Offset = 10,
                Limit = 50
            };

            var result = pagination.ToModel();

            result.Offset.Should().Be(pagination.Offset);
            result.Limit.Should().Be(pagination.Limit);
        }
    }
}
