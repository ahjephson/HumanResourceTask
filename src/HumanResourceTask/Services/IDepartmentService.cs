using FluentResults;
using HumanResourceTask.Models;

namespace HumanResourceTask.Services
{
    public interface IDepartmentService
    {
        Task<Result<IReadOnlyList<Department>>> GetDepartmentsAsync();
    }
}
