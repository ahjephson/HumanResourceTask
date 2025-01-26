using FluentResults;
using HumanResourceTask.Errors;
using HumanResourceTask.MetaModels;
using HumanResourceTask.Models;
using HumanResourceTask.Repositories;

namespace HumanResourceTask.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly TimeProvider _timeProvider;

        public EmployeeService(IEmployeeRepository employeeRepository, TimeProvider timeProvider)
        {
            _employeeRepository = employeeRepository;
            _timeProvider = timeProvider;
        }

        public Task<Result<EmployeeRecordView>> CreateEmployeeAsync(
            string firstName,
            string lastName,
            string email,
            DateOnly dateOfBirth,
            Guid departmentId,
            Guid statusId,
            long employeeNumber)
        {
            var record = new EmployeeRecord
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                DateOfBirth = dateOfBirth,
                DepartmentId = departmentId,
                StatusId = statusId,
                EmployeeNumber = employeeNumber,
                CreatedAtUtc = _timeProvider.GetUtcNow(),
            };

            return Result.Try(() => _employeeRepository.CreateEmployeeAsync(record), RepositoryErrorHandler.HandleError);
        }

        public async Task<Result> DeleteEmployeeAsync(Guid id)
        {
            var employeeResult = await GetEmployeeAsync(id);
            if (!employeeResult.IsSuccess)
            {
                return employeeResult.ToResult();
            }

            var employeeRecord = employeeResult.Value;

            var newRecord = employeeRecord with { Deleted = true };

            await UpdateEmployeeAsync(newRecord);

            return Result.Ok();
        }

        public async Task<Result<Paginated<EmployeeRecordView>>> ListEmployeeRecordsAsync(
            SortDefinition sortDefinition,
            PaginationDefinition paginationDefinition,
            Guid? statusId = null,
            Guid? departmentId = null)
        {
            var countResult = await Result.Try(() => _employeeRepository.CountEmployeeRecords(statusId, departmentId), RepositoryErrorHandler.HandleError);
            if (!countResult.IsSuccess)
            {
                return countResult.ToResult();
            }

            var count = countResult.Value;
            if (count == 0)
            {
                return Paginated<EmployeeRecordView>.Empty;
            }

            var listResult = await Result.Try(() => _employeeRepository.ListEmployeeRecords(sortDefinition, paginationDefinition, statusId, departmentId), RepositoryErrorHandler.HandleError);
            if (!listResult.IsSuccess)
            {
                return listResult.ToResult();
            }

            return new Paginated<EmployeeRecordView>(listResult.Value, count, paginationDefinition.Offset + listResult.Value.Count() < count);
        }

        public async Task<Result<EmployeeRecordView>> GetEmployeeAsync(Guid id)
        {
            var result = await Result.Try(() => _employeeRepository.FindEmployeeAsync(id), RepositoryErrorHandler.HandleError);
            if (!result.IsSuccess)
            {
                return result.ToResult();
            }

            if (result.Value is null)
            {
                return Result.Fail(new NotFoundError("Employee", id));
            }

            return result.Value;
        }

        public Task<Result<EmployeeRecordView>> UpdateEmployeeAsync(EmployeeRecord employeeRecord)
        {
            return Result.Try(() => _employeeRepository.UpdateEmployeeAsync(employeeRecord), RepositoryErrorHandler.HandleError);
        }
    }
}
