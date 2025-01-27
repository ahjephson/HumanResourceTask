using HumanResourceTask.Models;

namespace HumanResourceTask.Repositories
{
    public interface IStatusRepository
    {
        public Task<IEnumerable<Status>> ListStatusesAsync();
    }
}
