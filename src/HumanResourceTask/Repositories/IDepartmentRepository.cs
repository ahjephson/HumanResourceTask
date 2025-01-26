using HumanResourceTask.Models;

namespace HumanResourceTask.Repositories
{
    public interface IDepartmentRepository
    {
        public Task<IEnumerable<Department>> ListDepartmentsAsync();
    }
}
