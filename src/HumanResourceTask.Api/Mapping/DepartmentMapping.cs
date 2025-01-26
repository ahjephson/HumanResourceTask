using HumanResourceTask.Api.Dto.Department;
using HumanResourceTask.Models;

namespace HumanResourceTask.Api.Mapping
{
    public static class DepartmentMapping
    {
        public static DepartmentListItem ToListItemDto(this Department department)
        {
            return new DepartmentListItem
            {
                Id = department.Id,
                Name = department.Name,
            };
        }

        public static IEnumerable<DepartmentListItem> ToListItemDtos(this IEnumerable<Department> departments)
        {
            return departments.Select(ToListItemDto);
        }
    }
}
