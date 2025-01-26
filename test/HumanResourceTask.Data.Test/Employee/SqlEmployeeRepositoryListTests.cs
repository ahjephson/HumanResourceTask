using System.Data;
using FluentAssertions;
using HumanResourceTask.Data.Employee;
using HumanResourceTask.Exceptions;
using HumanResourceTask.MetaModels;
using HumanResourceTask.Models;
using Moq;

namespace HumanResourceTask.Data.Test.Employee
{
    public class SqlEmployeeRepositoryListTests
    {
        [Fact]
        public async Task GIVEN_ValidParameters_WHEN_ListEmployeeRecords_THEN_ShouldReturnExpectedRecords()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var sortDefinition = new SortDefinition { ColumnName = "LastName", Direction = SortDirection.Ascending };
            var paginationDefinition = new PaginationDefinition { Offset = 0, Limit = 10 };

            var expectedEmployees = new List<EmployeeRecordView>
            {
                new EmployeeRecordView
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
                    UpdatedAtUtc = null,
                    Deleted = false,
                    DepartmentName = "DepartmentName",
                    StatusName = "StatusName"
                },
                new EmployeeRecordView
                {
                    Id = Guid.NewGuid(),
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Email = "Email",
                    DateOfBirth = new DateOnly(1995, 5, 15),
                    DepartmentId = Guid.NewGuid(),
                    StatusId = Guid.NewGuid(),
                    EmployeeNumber = 654321,
                    CreatedAtUtc = DateTimeOffset.UtcNow,
                    UpdatedAtUtc = DateTimeOffset.UtcNow,
                    Deleted = false,
                    DepartmentName = "DepartmentName",
                    StatusName = "StatusName"
                }
            };

            mockDapperWrapper
                .Setup(d => d.QueryAsync<EmployeeRecordView>(
                    mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    null))
                .ReturnsAsync(expectedEmployees);

            mockConnectionFactory
                .Setup(factory => factory.Create())
                .Returns(mockConnection.Object);

            var repository = new SqlEmployeeRespository(mockConnectionFactory.Object, mockDapperWrapper.Object);

            var actualEmployees = await repository.ListEmployeeRecords(sortDefinition, paginationDefinition);

            actualEmployees.Should().BeEquivalentTo(expectedEmployees);
        }

        [Fact]
        public async Task GIVEN_SqlException_WHEN_ListEmployeeRecords_THEN_ShouldThrowRepositoryException()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockDapperWrapper = new Mock<IDapperWrapper>();

            var sortDefinition = new SortDefinition { ColumnName = "LastName", Direction = SortDirection.Ascending };
            var paginationDefinition = new PaginationDefinition { Offset = 0, Limit = 10 };

            var sqlException = new SqlExceptionBuilder()
                .WithErrorNumber(2627)
                .WithErrorMessage("A SQL exception occurred.")
                .Build();

            mockDapperWrapper
                .Setup(d => d.QueryAsync<EmployeeRecordView>(
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

            var exception = await Assert.ThrowsAsync<RepositoryException>(() => repository.ListEmployeeRecords(sortDefinition, paginationDefinition));

            exception.Message.Should().Be("A constraint violation occurred.");
        }
    }
}
