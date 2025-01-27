using FluentAssertions;
using FluentResults;
using HumanResourceTask.Errors;
using HumanResourceTask.Exceptions;
using HumanResourceTask.MetaModels;
using HumanResourceTask.Models;
using HumanResourceTask.Repositories;
using HumanResourceTask.Services;
using Moq;

namespace HumanResourceTask.Test.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly EmployeeService _target;

        public EmployeeServiceTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _target = new EmployeeService(_employeeRepositoryMock.Object, TimeProvider.System);
        }

        [Fact]
        public async Task GIVEN_ValidInput_WHEN_CreateEmployeeAsync_THEN_ShouldCreateEmployee()
        {
            var employee = new EmployeeRecordView
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                DepartmentName = "DepartmentName",
                StatusId = Guid.NewGuid(),
                StatusName = "StatusName",
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = null,
                Deleted = false
            };

            _employeeRepositoryMock
                .Setup(repo => repo.CreateEmployeeAsync(It.IsAny<EmployeeRecord>()))
                .ReturnsAsync(employee);

            var result = await _target.CreateEmployeeAsync(
                "FirstName",
                "LastName",
                "Email",
                employee.DateOfBirth,
                employee.DepartmentId,
                employee.StatusId,
                employee.EmployeeNumber);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(employee);
        }

        [Fact]
        public async Task GIVEN_ConstraintViolation_WHEN_CreateEmployeeAsync_THEN_ShouldReturnValidationError()
        {
            _employeeRepositoryMock
                .Setup(repo => repo.CreateEmployeeAsync(It.IsAny<EmployeeRecord>()))
                .ThrowsAsync(new RepositoryException(
                    "Unique constraint violated.",
                    RepositoryErrorType.ConstraintViolation,
                    columnName: "Email"));

            var result = await _target.CreateEmployeeAsync(
                "FirstName",
                "LastName",
                "Email",
                new DateOnly(1990, 1, 1),
                Guid.NewGuid(),
                Guid.NewGuid(),
                123456);

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle();
            var validationError = result.Errors.Single() as ValidationError;
            validationError.Should().NotBeNull();
            validationError!.Message.Should().Contain("Must be unique.");
            validationError.PropertyName.Should().Be("Email");
        }

        [Fact]
        public async Task GIVEN_UnknownError_WHEN_CreateEmployeeAsync_THEN_ShouldReturnGenericError()
        {
            _employeeRepositoryMock
                .Setup(repo => repo.CreateEmployeeAsync(It.IsAny<EmployeeRecord>()))
                .ThrowsAsync(new RepositoryException("Unknown error occurred.", RepositoryErrorType.Unknown));

            var result = await _target.CreateEmployeeAsync(
                "FirstName",
                "LastName",
                "Email",
                new DateOnly(1990, 1, 1),
                Guid.NewGuid(),
                Guid.NewGuid(),
                123456);

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle();
            var error = result.Errors.Single();
            error.Message.Should().Be("Unknown error occurred.");
        }

        [Fact]
        public async Task GIVEN_GenericException_WHEN_CreateEmployeeAsync_THEN_ShouldReturnExceptionalError()
        {
            _employeeRepositoryMock
                .Setup(repo => repo.CreateEmployeeAsync(It.IsAny<EmployeeRecord>()))
                .ThrowsAsync(new Exception("Unexpected error occurred."));

            var result = await _target.CreateEmployeeAsync(
                "FirstName",
                "LastName",
                "Email",
                new DateOnly(1990, 1, 1),
                Guid.NewGuid(),
                Guid.NewGuid(),
                123456);

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle();
            var exceptionalError = result.Errors.Single() as ExceptionalError;
            exceptionalError.Should().NotBeNull();
            exceptionalError!.Message.Should().Contain("Unexpected error occurred.");
            exceptionalError.Exception.Should().NotBeNull();
            exceptionalError.Exception.Message.Should().Be("Unexpected error occurred.");
        }

        [Fact]
        public async Task GIVEN_ValidInput_WHEN_UpdateEmployeeAsync_THEN_ShouldUpdateEmployee()
        {
            var employee = new EmployeeRecordView
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                DepartmentName = "DepartmentName",
                StatusId = Guid.NewGuid(),
                StatusName = "StatusName",
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = null,
                Deleted = false
            };

            _employeeRepositoryMock
                .Setup(repo => repo.UpdateEmployeeAsync(It.IsAny<EmployeeRecord>()))
                .ReturnsAsync(employee);

            var result = await _target.UpdateEmployeeAsync(employee);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(employee);
        }

        [Fact]
        public async Task GIVEN_ConstraintViolation_WHEN_UpdateEmployeeAsync_THEN_ShouldReturnValidationError()
        {
            _employeeRepositoryMock
                .Setup(repo => repo.UpdateEmployeeAsync(It.IsAny<EmployeeRecord>()))
                .ThrowsAsync(new RepositoryException(
                    "Unique constraint violated.",
                    RepositoryErrorType.ConstraintViolation,
                    columnName: "Email"));

            var employee = new EmployeeRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = null,
                Deleted = false
            };

            var result = await _target.UpdateEmployeeAsync(employee);

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle();
            var validationError = result.Errors.Single() as ValidationError;
            validationError.Should().NotBeNull();
            validationError!.Message.Should().Contain("Must be unique.");
            validationError.PropertyName.Should().Be("Email");
        }

        [Fact]
        public async Task GIVEN_GenericException_WHEN_UpdateEmployeeAsync_THEN_ShouldReturnExceptionalError()
        {
            _employeeRepositoryMock
                .Setup(repo => repo.UpdateEmployeeAsync(It.IsAny<EmployeeRecord>()))
                .ThrowsAsync(new Exception("Unexpected error occurred."));

            var employee = new EmployeeRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = null,
                Deleted = false
            };

            var result = await _target.UpdateEmployeeAsync(employee);

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle();
            var exceptionalError = result.Errors.Single() as ExceptionalError;
            exceptionalError.Should().NotBeNull();
            exceptionalError!.Message.Should().Contain("Unexpected error occurred.");
        }

        [Fact]
        public async Task GIVEN_NoRecords_WHEN_ListEmployeeRecords_THEN_ShouldReturnEmployeeViews()
        {
            _employeeRepositoryMock
                .Setup(repo => repo.CountEmployeeRecords(It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ReturnsAsync(0);

            _employeeRepositoryMock
                .Setup(repo => repo.ListEmployeeRecords(It.IsAny<SortDefinition>(), It.IsAny<PaginationDefinition>(), It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ReturnsAsync([]);

            var result = await _target.ListEmployeeRecordsAsync(SortDefinition.Defaults, PaginationDefinition.Defaults);

            result.IsSuccess.Should().BeTrue();
            result.Value.Items.Should().HaveCount(0);
            result.Value.Total.Should().Be(0);
            result.Value.HasMore.Should().Be(false);
        }

        [Fact]
        public async Task GIVEN_CountSucceedsButListFails_WHEN_ListEmployeeRecords_THEN_ShouldReturnEmployeeViews()
        {
            _employeeRepositoryMock
                .Setup(repo => repo.CountEmployeeRecords(It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ReturnsAsync(3);

            _employeeRepositoryMock
                .Setup(repo => repo.ListEmployeeRecords(It.IsAny<SortDefinition>(), It.IsAny<PaginationDefinition>(), It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ThrowsAsync(new RepositoryException("Repository failure", RepositoryErrorType.SqlException, innerException: new Exception("Database connection error")));

            var result = await _target.ListEmployeeRecordsAsync(SortDefinition.Defaults, PaginationDefinition.Defaults);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            var exceptionalError = result.Errors.Single() as ExceptionalError;
            exceptionalError.Should().NotBeNull();
            exceptionalError!.Message.Should().Be("Database connection error");
            exceptionalError.Exception.Should().NotBeNull();
            exceptionalError.Exception.Message.Should().Be("Database connection error");
        }

        [Fact]
        public async Task GIVEN_ValidFilters_WHEN_ListEmployeeRecords_THEN_ShouldReturnEmployeeViews()
        {
            var employees = new List<EmployeeRecordView>
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
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = null,
                    Deleted = false,
                    DepartmentName = "DepartmentName",
                    StatusName = "StatusName"
                }
            };

            _employeeRepositoryMock
                .Setup(repo => repo.CountEmployeeRecords(It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ReturnsAsync(employees.Count);

            _employeeRepositoryMock
                .Setup(repo => repo.ListEmployeeRecords(It.IsAny<SortDefinition>(), It.IsAny<PaginationDefinition>(), It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ReturnsAsync(employees);

            var result = await _target.ListEmployeeRecordsAsync(SortDefinition.Defaults, PaginationDefinition.Defaults);

            result.IsSuccess.Should().BeTrue();
            result.Value.Items.Should().BeEquivalentTo(employees);
            result.Value.Total.Should().Be(employees.Count);
            result.Value.HasMore.Should().Be(false);
        }

        [Fact]
        public async Task GIVEN_ValidFiltersBelowPageSize_WHEN_ListEmployeeRecords_THEN_ShouldReturnEmployeeViews()
        {
            var employees = new List<EmployeeRecordView>
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
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = null,
                    Deleted = false,
                    DepartmentName = "DepartmentName",
                    StatusName = "StatusName"
                }
            };

            _employeeRepositoryMock
                .Setup(repo => repo.CountEmployeeRecords(It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ReturnsAsync(2);

            _employeeRepositoryMock
                .Setup(repo => repo.ListEmployeeRecords(It.IsAny<SortDefinition>(), It.IsAny<PaginationDefinition>(), It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ReturnsAsync(employees);

            var result = await _target.ListEmployeeRecordsAsync(SortDefinition.Defaults, new PaginationDefinition { Limit = 1, Offset = 0 });

            result.IsSuccess.Should().BeTrue();
            result.Value.Items.Should().BeEquivalentTo(employees);
            result.Value.Total.Should().Be(2);
            result.Value.HasMore.Should().Be(true);
        }

        [Fact]
        public async Task GIVEN_RepositoryFailure_WHEN_ListEmployeeRecords_THEN_ShouldReturnExceptionalError()
        {
            _employeeRepositoryMock
                .Setup(repo => repo.CountEmployeeRecords(It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ThrowsAsync(new RepositoryException("Repository failure", RepositoryErrorType.SqlException, innerException: new Exception("Database connection error")));

            _employeeRepositoryMock
                .Setup(repo => repo.ListEmployeeRecords(It.IsAny<SortDefinition>(), It.IsAny<PaginationDefinition>(), It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ThrowsAsync(new RepositoryException("Repository failure", RepositoryErrorType.SqlException, innerException: new Exception("Database connection error")));

            var result = await _target.ListEmployeeRecordsAsync(SortDefinition.Defaults, PaginationDefinition.Defaults);

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle();
            var exceptionalError = result.Errors.Single() as ExceptionalError;
            exceptionalError.Should().NotBeNull();
            exceptionalError!.Message.Should().Be("Database connection error");
            exceptionalError.Exception.Should().NotBeNull();
            exceptionalError.Exception.Message.Should().Be("Database connection error");
        }

        [Fact]
        public async Task GIVEN_GenericException_WHEN_ListEmployeeRecords_THEN_ShouldReturnExceptionalError()
        {
            _employeeRepositoryMock
                .Setup(repo => repo.CountEmployeeRecords(It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ThrowsAsync(new Exception("Unexpected error occurred."));

            _employeeRepositoryMock
                .Setup(repo => repo.ListEmployeeRecords(It.IsAny<SortDefinition>(), It.IsAny<PaginationDefinition>(), It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .ThrowsAsync(new Exception("Unexpected error occurred."));

            var result = await _target.ListEmployeeRecordsAsync(SortDefinition.Defaults, PaginationDefinition.Defaults);

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle();
            var exceptionalError = result.Errors.Single() as ExceptionalError;
            exceptionalError.Should().NotBeNull();
            exceptionalError!.Message.Should().Contain("Unexpected error occurred.");
        }

        [Fact]
        public async Task GIVEN_ValidId_WHEN_GetEmployeeAsync_THEN_ShouldReturnEmployeeView()
        {
            var employeeView = new EmployeeRecordView
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = null,
                Deleted = false,
                DepartmentName = "DepartmentName",
                StatusName = "StatusName"
            };

            _employeeRepositoryMock
                .Setup(repo => repo.FindEmployeeAsync(employeeView.Id))
                .ReturnsAsync(employeeView);

            var result = await _target.GetEmployeeAsync(employeeView.Id);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(employeeView);
        }

        [Fact]
        public async Task GIVEN_ValidInputs_WHEN_DeleteEmployeeAsync_THEN_ShouldMarkAsDeleted()
        {
            var employeeView = new EmployeeRecordView
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = null,
                Deleted = false,
                DepartmentName = "DepartmentName",
                StatusName = "StatusName"
            };

            var updatedEmployee = new EmployeeRecordView
            {
                Id = employeeView.Id,
                FirstName = employeeView.FirstName,
                LastName = employeeView.LastName,
                Email = employeeView.Email,
                DateOfBirth = employeeView.DateOfBirth,
                DepartmentId = employeeView.DepartmentId,
                DepartmentName = employeeView.DepartmentName,
                StatusId = employeeView.StatusId,
                StatusName = employeeView.StatusName,
                EmployeeNumber = employeeView.EmployeeNumber,
                CreatedAtUtc = employeeView.CreatedAtUtc,
                UpdatedAtUtc = DateTime.UtcNow,
                Deleted = true
            };

            _employeeRepositoryMock
                .Setup(repo => repo.FindEmployeeAsync(employeeView.Id))
                .ReturnsAsync(employeeView);

            _employeeRepositoryMock
                .Setup(repo => repo.UpdateEmployeeAsync(It.IsAny<EmployeeRecord>()))
                .ReturnsAsync(updatedEmployee);

            var result = await _target.DeleteEmployeeAsync(employeeView.Id);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GIVEN_RepositoryFailure_WHEN_GetEmployeeAsync_THEN_ShouldReturnExceptionalError()
        {
            _employeeRepositoryMock
                .Setup(repo => repo.FindEmployeeAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new RepositoryException("Repository failure", RepositoryErrorType.SqlException, innerException: new Exception("Database connection error")));

            var result = await _target.GetEmployeeAsync(Guid.NewGuid());

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle();
            var exceptionalError = result.Errors.Single() as ExceptionalError;
            exceptionalError.Should().NotBeNull();
            exceptionalError!.Message.Should().Be("Database connection error");
            exceptionalError.Exception.Should().NotBeNull();
            exceptionalError.Exception.Message.Should().Be("Database connection error");
        }

        [Fact]
        public async Task GIVEN_GetEmployeeFailure_WHEN_DeleteEmployeeAsync_THEN_ShouldReturnNotFoundError()
        {
            _employeeRepositoryMock
                .Setup(repo => repo.FindEmployeeAsync(It.IsAny<Guid>()))
                .ReturnsAsync((EmployeeRecordView?)null);

            var result = await _target.DeleteEmployeeAsync(Guid.NewGuid());

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e is NotFoundError);
        }
    }
}
