using System.Data;
using FluentAssertions;
using HumanResourceTask.Data.Status;
using HumanResourceTask.Exceptions;
using Moq;

namespace HumanResourceTask.Data.Test.Status
{
    public class SqlStatusRepositoryTests
    {
        [Fact]
        public async Task ListStatusesAsync_ReturnsExpectedStatuses()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var expectedStatuses = new List<Models.Status>
            {
                new Models.Status { Id = Guid.NewGuid(), Name = "Name1" },
                new Models.Status { Id = Guid.NewGuid(), Name = "Name2" }
            };

            mockDapperWrapper
                .Setup(d => d.QueryAsync<Models.Status>(mockConnection.Object, It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(expectedStatuses);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlStatusRepository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var actualStatuses = await repository.ListStatusesAsync();

            actualStatuses.Should().BeEquivalentTo(expectedStatuses);
        }

        [Fact]
        public async Task ListStatusesAsync_ThrowsRepositoryException_OnSqlException()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var sqlException = new SqlExceptionBuilder()
                .WithErrorNumber(2627)
                .WithErrorMessage("Cannot insert duplicate key row in object 'dbo.Table' with unique index 'UQ_Table_Column'. The duplicate key value is (value).")
                .Build();

            mockDapperWrapper
                .Setup(d => d.QueryAsync<Models.Status>(mockConnection.Object, It.IsAny<string>(), null, null, null, null))
                .ThrowsAsync(sqlException);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlStatusRepository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var exception = await Assert.ThrowsAsync<RepositoryException>(() => repository.ListStatusesAsync());

            exception.Message.Should().Be("A constraint violation occurred.");
            exception.ErrorType.Should().Be(RepositoryErrorType.ConstraintViolation);
        }
    }
}
