using FluentResults;
using HumanResourceTask.Models;

namespace HumanResourceTask.Services
{
    public interface IStatusService
    {
        Task<Result<IReadOnlyList<Status>>> GetStatusesAsync();
    }
}
