using System.Data;
using FluentAssertions;
using HumanResourceTask.Data.Department;
using HumanResourceTask.Exceptions;
using Moq;

namespace HumanResourceTask.Data.Test.Department
{
    public class SqlDepartmentRepositoryTests
    {
        [Fact]
        public async Task ListDepartmentesAsync_ReturnsExpectedDepartmentes()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var expectedDepartmentes = new List<Models.Department>
            {
                new Models.Department { Id = Guid.NewGuid(), Name = "Name1" },
                new Models.Department { Id = Guid.NewGuid(), Name = "Name2" }
            };

            mockDapperWrapper
                .Setup(d => d.QueryAsync<Models.Department>(mockConnection.Object, It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(expectedDepartmentes);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlDepartmentRepository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var actualDepartmentes = await repository.ListDepartmentsAsync();

            actualDepartmentes.Should().BeEquivalentTo(expectedDepartmentes);
        }

        [Fact]
        public async Task ListDepartmentesAsync_ThrowsRepositoryException_OnSqlException()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var sqlException = new SqlExceptionBuilder()
                .WithErrorNumber(2627)
                .WithErrorMessage("Cannot insert duplicate key row in object 'dbo.Table' with unique index 'UQ_Table_Column'. The duplicate key value is (value).")
                .Build();

            mockDapperWrapper
                .Setup(d => d.QueryAsync<Models.Department>(mockConnection.Object, It.IsAny<string>(), null, null, null, null))
                .ThrowsAsync(sqlException);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlDepartmentRepository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var exception = await Assert.ThrowsAsync<RepositoryException>(() => repository.ListDepartmentsAsync());

            exception.Message.Should().Be("A constraint violation occurred.");
            exception.ErrorType.Should().Be(RepositoryErrorType.ConstraintViolation);
        }
    }
}
