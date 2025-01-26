using System.Data;
using FluentAssertions;
using Moq;

namespace HumanResourceTask.Data.Test
{
    public class DateOnlyTypeHandlerTests
    {
        [Fact]
        public void GIVEN_DateTimeValue_WHEN_ParseIsCalled_THEN_ShouldReturnExpectedDateOnly()
        {
            var dateTimeValue = new DateTime(2025, 1, 1, 14, 30, 0);
            var handler = new DateOnlyTypeHandler();

            var result = handler.Parse(dateTimeValue);

            result.Should().Be(new DateOnly(2025, 1, 1));
        }

        [Fact]
        public void GIVEN_DateOnlyValue_WHEN_SetValueIsCalled_THEN_ShouldSetParameterCorrectly()
        {
            var parameterMock = new Mock<IDbDataParameter>();
            var handler = new DateOnlyTypeHandler();

            var dateOnlyValue = new DateOnly(2025, 1, 1);

            handler.SetValue(parameterMock.Object, dateOnlyValue);

            parameterMock.VerifySet(p => p.DbType = DbType.Date);
            parameterMock.VerifySet(p => p.Value = dateOnlyValue);
        }

        [Fact]
        public void GIVEN_InvalidValue_WHEN_ParseIsCalled_THEN_ShouldThrowInvalidCastException()
        {
            var handler = new DateOnlyTypeHandler();
            var invalidValue = "InvalidDate";

            Action act = () => handler.Parse(invalidValue);

            act.Should().Throw<InvalidCastException>();
        }
    }
}
