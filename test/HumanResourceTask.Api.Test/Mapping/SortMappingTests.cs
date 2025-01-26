using FluentAssertions;
using HumanResourceTask.Api.Dto;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.MetaModels;
using Xunit;

namespace HumanResourceTask.Api.Test.Mapping
{
    public class SortMappingTests
    {
        [Fact]
        public void GIVEN_NullSort_WHEN_ToModelIsCalled_THEN_ShouldReturnDefaultSortDefinition()
        {
            Sort? sort = null;

            var result = sort.ToModel();

            result.Should().BeEquivalentTo(SortDefinition.Defaults);
        }

        [Fact]
        public void GIVEN_ValidSortWithAscendingTrue_WHEN_ToModelIsCalled_THEN_ShouldMapToAscendingSortDefinition()
        {
            var sort = new Sort
            {
                Column = "Column",
                Ascending = true
            };

            var result = sort.ToModel();

            result.ColumnName.Should().Be("Column");
            result.Direction.Should().Be(SortDirection.Ascending);
        }

        [Fact]
        public void GIVEN_ValidSortWithAscendingFalse_WHEN_ToModelIsCalled_THEN_ShouldMapToDescendingSortDefinition()
        {
            var sort = new Sort
            {
                Column = "Column",
                Ascending = false
            };

            var result = sort.ToModel();

            result.ColumnName.Should().Be("Column");
            result.Direction.Should().Be(SortDirection.Descending);
        }
    }
}
