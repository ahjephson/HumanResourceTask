using FluentAssertions;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.Models;

namespace HumanResourceTask.Api.Test.Mapping
{
    public class EmployeeRecordMappingTests
    {
        [Fact]
        public void GIVEN_EmployeeRecord_WHEN_Mapping_THEN_ShouldMapCorrectly()
        {
            var record = new EmployeeRecordView
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                DepartmentName = "DepartmentName",
                StatusId = Guid.NewGuid(),
                StatusName = "StatusName",
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow.AddHours(1),
                Deleted = false
            };

            var response = record.ToResponseDto();

            response.Should().BeEquivalentTo(record);
        }
    }
}
