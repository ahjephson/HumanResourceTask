using FluentAssertions;
using HumanResourceTask.Api.Dto.Department;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.Models;

namespace HumanResourceTask.Api.Test.Mapping
{
    public class DepartmentMappingTests
    {
        [Fact]
        public void GIVEN_Department_WHEN_ToListItemDtoIsCalled_THEN_ShouldMapCorrectly()
        {
            var department = new Department
            {
                Id = Guid.NewGuid(),
                Name = "Department1"
            };

            var result = department.ToListItemDto();

            result.Id.Should().Be(department.Id);
            result.Name.Should().Be(department.Name);
        }

        [Fact]
        public void GIVEN_DepartmentCollection_WHEN_ToListItemDtosIsCalled_THEN_ShouldMapAllDepartmentsCorrectly()
        {
            var departments = new List<Department>
            {
                new Department
                {
                    Id = Guid.NewGuid(),
                    Name = "Department1"
                },
                new Department
                {
                    Id = Guid.NewGuid(),
                    Name = "Department2"
                }
            };

            var result = departments.ToListItemDtos();

            result.Should().HaveCount(2);
            result.Should().ContainEquivalentOf(new DepartmentListItem
            {
                Id = departments[0].Id,
                Name = departments[0].Name
            });
            result.Should().ContainEquivalentOf(new DepartmentListItem
            {
                Id = departments[1].Id,
                Name = departments[1].Name
            });
        }
    }
}
