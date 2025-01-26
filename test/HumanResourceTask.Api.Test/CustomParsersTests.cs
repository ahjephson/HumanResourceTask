using FluentAssertions;
using HumanResourceTask.Api.Dto;
using Microsoft.Extensions.Primitives;

namespace HumanResourceTask.Api.Test
{
    public class CustomParsersTests
    {
        [Fact]
        public void GIVEN_NullArgument_WHEN_SortParserIsCalled_THEN_ShouldReturnParseResultWithSuccessFalse()
        {
            object? arg = null;

            var result = CustomParsers.SortParser(arg);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
        }

        [Fact]
        public void GIVEN_EmptyStringValuesArgument_WHEN_SortParserIsCalled_THEN_ShouldReturnParseResultWithSuccessFalse()
        {
            object? arg = StringValues.Empty;

            var result = CustomParsers.SortParser(arg);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
        }

        [Fact]
        public void GIVEN_InvalidArgumentType_WHEN_SortParserIsCalled_THEN_ShouldReturnParseResultWithSuccessFalse()
        {
            object? arg = new object();

            var result = CustomParsers.SortParser(arg);

            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
        }

        [Fact]
        public void GIVEN_ValidStringValuesWithNoPrefix_WHEN_SortParserIsCalled_THEN_ShouldReturnParseResultWithAscendingTrue()
        {
            object? arg = new StringValues("ColumnName");

            var result = CustomParsers.SortParser(arg);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<Sort>();
            var sort = (Sort)result.Value!;
            sort.Ascending.Should().BeTrue();
            sort.Column.Should().Be("ColumnName");
        }

        [Fact]
        public void GIVEN_ValidStringValuesWithPlusPrefix_WHEN_SortParserIsCalled_THEN_ShouldReturnParseResultWithAscendingTrue()
        {
            object? arg = new StringValues("+ColumnName");

            var result = CustomParsers.SortParser(arg);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<Sort>();
            var sort = (Sort)result.Value!;
            sort.Ascending.Should().BeTrue();
            sort.Column.Should().Be("ColumnName");
        }

        [Fact]
        public void GIVEN_ValidStringValuesWithMinusPrefix_WHEN_SortParserIsCalled_THEN_ShouldReturnParseResultWithAscendingFalse()
        {
            object? arg = new StringValues("-ColumnName");

            var result = CustomParsers.SortParser(arg);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<Sort>();
            var sort = (Sort)result.Value!;
            sort.Ascending.Should().BeFalse();
            sort.Column.Should().Be("ColumnName");
        }
    }
}
