using FluentResults;
using HumanResourceTask.Models;
using HumanResourceTask.Repositories;

namespace HumanResourceTask.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public Task<Result<IReadOnlyList<Department>>> GetDepartmentsAsync()
        {
            return Result.Try(GetDepartments, RepositoryErrorHandler.HandleError);
        }

        private async Task<IReadOnlyList<Department>> GetDepartments()
        {
            return (await _departmentRepository.ListDepartmentsAsync()).ToList().AsReadOnly();
        }
    }
}
