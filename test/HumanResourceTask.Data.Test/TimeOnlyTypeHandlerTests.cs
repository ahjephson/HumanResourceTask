using System.Data;
using FluentAssertions;
using Moq;

namespace HumanResourceTask.Data.Test
{
    public class TimeOnlyTypeHandlerTests
    {
        [Fact]
        public void GIVEN_DateTimeValue_WHEN_ParseIsCalled_THEN_ShouldReturnExpectedTimeOnly()
        {
            var dateTimeValue = new DateTime(2025, 1, 1, 14, 30, 0);
            var handler = new TimeOnlyTypeHandler();

            var result = handler.Parse(dateTimeValue);

            result.Should().Be(new TimeOnly(14, 30));
        }

        [Fact]
        public void GIVEN_TimeOnlyValue_WHEN_SetValueIsCalled_THEN_ShouldSetParameterCorrectly()
        {
            var parameterMock = new Mock<IDbDataParameter>();
            var handler = new TimeOnlyTypeHandler();

            var timeOnlyValue = new TimeOnly(14, 30);

            handler.SetValue(parameterMock.Object, timeOnlyValue);

            parameterMock.VerifySet(p => p.DbType = DbType.Time);
            parameterMock.VerifySet(p => p.Value = timeOnlyValue);
        }

        [Fact]
        public void GIVEN_InvalidValue_WHEN_ParseIsCalled_THEN_ShouldThrowInvalidCastException()
        {
            var handler = new TimeOnlyTypeHandler();
            var invalidValue = "InvalidTime";

            Action act = () => handler.Parse(invalidValue);

            act.Should().Throw<InvalidCastException>();
        }
    }
}
