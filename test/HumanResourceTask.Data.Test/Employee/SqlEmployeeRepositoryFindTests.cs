using System.Data;
using FluentAssertions;
using HumanResourceTask.Data.Employee;
using HumanResourceTask.Exceptions;
using HumanResourceTask.Models;
using Moq;

namespace HumanResourceTask.Data.Test.Employee
{
    public class SqlEmployeeRepositoryFindTests
    {
        [Fact]
        public async Task GIVEN_ExistingEmployeeId_WHEN_FindEmployeeAsync_THEN_ShouldReturnExpectedEmployee()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var employeeId = Guid.NewGuid();

            var expectedEmployee = new EmployeeRecordView
            {
                Id = employeeId,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = null,
                Deleted = false,
                DepartmentName = "DepartmentName",
                StatusName = "StatusName"
            };

            mockDapperWrapper
                .Setup(d => d.QueryFirstOrDefaultAsync<EmployeeRecordView>(mockConnection.Object, It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(expectedEmployee);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlEmployeeRespository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var actualEmployee = await repository.FindEmployeeAsync(employeeId);

            actualEmployee.Should().BeEquivalentTo(expectedEmployee);
        }

        [Fact]
        public async Task GIVEN_NonExistingEmployeeId_WHEN_FindEmployeeAsync_THEN_ShouldReturnNull()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var employeeId = Guid.NewGuid();

            mockDapperWrapper
                .Setup(d => d.QueryFirstOrDefaultAsync<EmployeeRecordView>(mockConnection.Object, It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync((EmployeeRecordView?)null);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlEmployeeRespository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var actualEmployee = await repository.FindEmployeeAsync(employeeId);

            actualEmployee.Should().BeNull();
        }

        [Fact]
        public async Task GIVEN_SqlException_WHEN_FindEmployeeAsync_THEN_ShouldThrowRepositoryException()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var employeeId = Guid.NewGuid();

            var sqlException = new SqlExceptionBuilder()
                .WithErrorNumber(2627)
                .WithErrorMessage("A SQL exception occurred.")
                .Build();

            mockDapperWrapper
                .Setup(d => d.QueryFirstOrDefaultAsync<EmployeeRecordView>(mockConnection.Object, It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ThrowsAsync(sqlException);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlEmployeeRespository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var exception = await Assert.ThrowsAsync<RepositoryException>(() => repository.FindEmployeeAsync(employeeId));

            exception.Message.Should().Be("A constraint violation occurred.");
        }
    }
}
