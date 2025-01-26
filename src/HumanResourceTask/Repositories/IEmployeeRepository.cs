using HumanResourceTask.MetaModels;
using HumanResourceTask.Models;

namespace HumanResourceTask.Repositories
{
    public interface IEmployeeRepository
    {
        public Task<EmployeeRecordView> CreateEmployeeAsync(EmployeeRecord employeeRecord);

        public Task<EmployeeRecordView?> FindEmployeeAsync(Guid id);

        public Task<IEnumerable<EmployeeRecordView>> ListEmployeeRecords(
            SortDefinition sortDefinition,
            PaginationDefinition paginationDefinition,
            Guid? statusId = null,
            Guid? departmentId = null);

        public Task<long> CountEmployeeRecords(
            Guid? statusId = null,
            Guid? departmentId = null);

        public Task<EmployeeRecordView> UpdateEmployeeAsync(EmployeeRecord employeeRecord);
    }
}
