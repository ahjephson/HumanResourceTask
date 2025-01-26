using HumanResourceTask.Api.Dto.Status;
using HumanResourceTask.Models;

namespace HumanResourceTask.Api.Mapping
{
    public static class StatusMapping
    {
        public static StatusListItem ToListItemDto(this Status status)
        {
            return new StatusListItem
            {
                Id = status.Id,
                Name = status.Name,
            };
        }

        public static IEnumerable<StatusListItem> ToListItemDtos(this IEnumerable<Status> statuses)
        {
            return statuses.Select(ToListItemDto);
        }
    }
}
