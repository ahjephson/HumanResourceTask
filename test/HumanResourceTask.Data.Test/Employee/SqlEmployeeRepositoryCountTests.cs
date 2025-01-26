using System.Data;
using FluentAssertions;
using HumanResourceTask.Data.Employee;
using HumanResourceTask.Exceptions;
using Moq;

namespace HumanResourceTask.Data.Test.Employee
{
    public class SqlEmployeeRepositoryCountTests
    {
        [Fact]
        public async Task GIVEN_ValidParameters_WHEN_CountEmployeeRecords_THEN_ShouldReturnExpectedCount()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var expectedCount = 42;

            mockDapperWrapper
                .Setup(d => d.ExecuteScalarAsync<long>(
                    mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    null))
                .ReturnsAsync(expectedCount);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlEmployeeRespository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var actualCount = await repository.CountEmployeeRecords();

            actualCount.Should().Be(expectedCount);
        }

        [Fact]
        public async Task GIVEN_SqlException_WHEN_CountEmployeeRecords_THEN_ShouldThrowRepositoryException()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var sqlException = new SqlExceptionBuilder()
                .WithErrorNumber(2627)
                .WithErrorMessage("A SQL exception occurred.")
                .Build();

            mockDapperWrapper
                .Setup(d => d.ExecuteScalarAsync<long>(
                    mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    null))
                .ThrowsAsync(sqlException);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlEmployeeRespository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var exception = await Assert.ThrowsAsync<RepositoryException>(() => repository.CountEmployeeRecords());

            exception.Message.Should().Be("A constraint violation occurred.");
        }
    }
}
