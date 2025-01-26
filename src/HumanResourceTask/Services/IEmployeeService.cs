using FluentResults;
using HumanResourceTask.MetaModels;
using HumanResourceTask.Models;

namespace HumanResourceTask.Services
{
    public interface IEmployeeService
    {
        public Task<Result<EmployeeRecordView>> GetEmployeeAsync(Guid id);

        public Task<Result<EmployeeRecordView>> CreateEmployeeAsync(
            string firstName,
            string lastName,
            string email,
            DateOnly dateOfBirth,
            Guid departmentId,
            Guid statusId,
            long employeeNumber);

        public Task<Result<EmployeeRecordView>> UpdateEmployeeAsync(EmployeeRecord employeeRecord);

        public Task<Result> DeleteEmployeeAsync(Guid Id);

        public Task<Result<Paginated<EmployeeRecordView>>> ListEmployeeRecordsAsync(
            SortDefinition sortDefinition,
            PaginationDefinition paginationDefinition,
            Guid? statusId = null,
            Guid? departmentId = null);
    }
}
