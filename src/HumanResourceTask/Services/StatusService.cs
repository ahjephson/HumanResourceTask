using FluentResults;
using HumanResourceTask.Models;
using HumanResourceTask.Repositories;

namespace HumanResourceTask.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;

        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public Task<Result<IReadOnlyList<Status>>> GetStatusesAsync()
        {
            return Result.Try(GetStatuses, RepositoryErrorHandler.HandleError);
        }

        private async Task<IReadOnlyList<Status>> GetStatuses()
        {
            return (await _statusRepository.ListStatusesAsync()).ToList().AsReadOnly();
        }
    }
}
