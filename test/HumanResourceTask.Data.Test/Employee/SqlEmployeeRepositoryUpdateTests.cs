using System.Data;
using FluentAssertions;
using HumanResourceTask.Data.Employee;
using HumanResourceTask.Exceptions;
using HumanResourceTask.Models;
using Moq;

namespace HumanResourceTask.Data.Test.Employee
{
    public class SqlEmployeeRepositoryUpdateTests
    {
        [Fact]
        public async Task GIVEN_ValidEmployeeRecord_WHEN_UpdateEmployeeAsync_THEN_ShouldReturnUpdatedEmployee()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var employeeRecord = new EmployeeRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow,
                Deleted = false
            };

            var updatedEmployee = new EmployeeRecordView
            {
                Id = employeeRecord.Id,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = employeeRecord.DateOfBirth,
                DepartmentId = employeeRecord.DepartmentId,
                StatusId = employeeRecord.StatusId,
                EmployeeNumber = employeeRecord.EmployeeNumber,
                CreatedAtUtc = employeeRecord.CreatedAtUtc,
                UpdatedAtUtc = employeeRecord.UpdatedAtUtc,
                Deleted = employeeRecord.Deleted,
                DepartmentName = "DepartmentName",
                StatusName = "StatusName"
            };

            mockDapperWrapper
                .Setup(d => d.ExecuteAsync(mockConnection.Object, It.IsAny<string>(), employeeRecord, null, null, null))
                .ReturnsAsync(1);

            mockDapperWrapper
                .Setup(d => d.QueryFirstOrDefaultAsync<EmployeeRecordView>(mockConnection.Object, It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync(updatedEmployee);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlEmployeeRespository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var actualEmployee = await repository.UpdateEmployeeAsync(employeeRecord);

            actualEmployee.Should().BeEquivalentTo(updatedEmployee);
        }

        [Fact]
        public async Task GIVEN_RowsAffectedIsZero_WHEN_UpdateEmployeeAsync_THEN_ShouldThrowRepositoryException()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var employeeRecord = new EmployeeRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow,
                Deleted = false
            };

            mockDapperWrapper
                .Setup(d => d.ExecuteAsync(mockConnection.Object, It.IsAny<string>(), employeeRecord, null, null, null))
                .ReturnsAsync(0);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlEmployeeRespository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var exception = await Assert.ThrowsAsync<RepositoryException>(() => repository.UpdateEmployeeAsync(employeeRecord));

            exception.Message.Should().Be("Failed to update the employee record.");
        }

        [Fact]
        public async Task GIVEN_NullRecord_WHEN_UpdateEmployeeAsync_THEN_ShouldThrowRepositoryException()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var employeeRecord = new EmployeeRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow,
                Deleted = false
            };

            mockDapperWrapper
                .Setup(d => d.ExecuteAsync(mockConnection.Object, It.IsAny<string>(), employeeRecord, null, null, null))
                .ReturnsAsync(1);

            mockDapperWrapper
                .Setup(d => d.QueryFirstOrDefaultAsync<EmployeeRecordView>(mockConnection.Object, It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                .ReturnsAsync((EmployeeRecordView?)null);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlEmployeeRespository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var exception = await Assert.ThrowsAsync<RepositoryException>(() => repository.UpdateEmployeeAsync(employeeRecord));

            exception.Message.Should().Be("The updated employee record could not be retrieved.");
        }

        [Fact]
        public async Task GIVEN_SqlException_WHEN_UpdateEmployeeAsync_THEN_ShouldThrowRepositoryException()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var employeeRecord = new EmployeeRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow,
                Deleted = false
            };

            var sqlException = new SqlExceptionBuilder()
                .WithErrorNumber(2627)
                .WithErrorMessage("A SQL exception occurred.")
                .Build();

            mockDapperWrapper
                .Setup(d => d.ExecuteAsync(mockConnection.Object, It.IsAny<string>(), employeeRecord, null, null, null))
                .ThrowsAsync(sqlException);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlEmployeeRespository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var exception = await Assert.ThrowsAsync<RepositoryException>(() => repository.UpdateEmployeeAsync(employeeRecord));

            exception.Message.Should().Be("A constraint violation occurred.");
        }
    }
}
