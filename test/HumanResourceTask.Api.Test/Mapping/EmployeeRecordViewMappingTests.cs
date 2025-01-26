using FluentAssertions;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Api.Mapping;
using HumanResourceTask.Models;

namespace HumanResourceTask.Api.Test.Mapping
{
    public class EmployeeMappingTests
    {
        [Fact]
        public void GIVEN_EmployeeRecordView_WHEN_ToResponseDtoIsCalled_THEN_ShouldMapCorrectly()
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

        [Fact]
        public void GIVEN_EmployeeRecordView_WHEN_ToListItemDtoIsCalled_THEN_ShouldMapCorrectly()
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

            var listItem = record.ToListItemDto();

            listItem.Should().BeEquivalentTo(new EmployeeListItem
            {
                Id = record.Id,
                FirstName = record.FirstName,
                LastName = record.LastName,
                DateOfBirth = record.DateOfBirth,
                DepartmentName = record.DepartmentName,
                StatusName = record.StatusName,
                EmployeeNumber = record.EmployeeNumber
            });
        }

        [Fact]
        public void GIVEN_MultipleEmployeeRecordViews_WHEN_ToListItemDtosIsCalled_THEN_ShouldMapAllRecordsCorrectly()
        {
            var records = new List<EmployeeRecordView>
            {
                new EmployeeRecordView
                {
                    Id = Guid.NewGuid(),
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Email = "Email1",
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    DepartmentId = Guid.NewGuid(),
                    DepartmentName = "DepartmentName1",
                    StatusId = Guid.NewGuid(),
                    StatusName = "StatusName1",
                    EmployeeNumber = 123456,
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = DateTime.UtcNow.AddHours(1),
                    Deleted = false
                },
                new EmployeeRecordView
                {
                    Id = Guid.NewGuid(),
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Email = "Email2",
                    DateOfBirth = new DateOnly(1991, 1, 1),
                    DepartmentId = Guid.NewGuid(),
                    DepartmentName = "DepartmentName2",
                    StatusId = Guid.NewGuid(),
                    StatusName = "StatusName2",
                    EmployeeNumber = 789123,
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = DateTime.UtcNow.AddHours(2),
                    Deleted = true
                }
            };

            var listItems = records.ToListItemDtos().ToList();

            listItems.Should().HaveCount(2);
            listItems[0].Should().BeEquivalentTo(records[0].ToListItemDto());
            listItems[1].Should().BeEquivalentTo(records[1].ToListItemDto());
        }

        [Fact]
        public void GIVEN_UpdateEmployeeRequest_WHEN_ToModelIsCalled_THEN_ShouldMapCorrectly()
        {
            var employeeRecord = new EmployeeRecord
            {
                Id = Guid.NewGuid(),
                FirstName = "OriginalFirstName",
                LastName = "OriginalLastName",
                Email = "OriginalEmail",
                DateOfBirth = new DateOnly(1990, 1, 1),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 123456,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow.AddHours(1),
                Deleted = false
            };

            var request = new UpdateEmployeeRequest
            {
                Id = employeeRecord.Id,
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Email = "UpdatedEmail",
                DateOfBirth = new DateOnly(1995, 12, 25),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 789123
            };

            var updatedRecord = request.ToModel(employeeRecord);

            updatedRecord.FirstName.Should().Be(request.FirstName);
            updatedRecord.LastName.Should().Be(request.LastName);
            updatedRecord.Email.Should().Be(request.Email);
            updatedRecord.DateOfBirth.Should().Be(request.DateOfBirth);
            updatedRecord.DepartmentId.Should().Be(request.DepartmentId.Value);
            updatedRecord.StatusId.Should().Be(request.StatusId.Value);
            updatedRecord.EmployeeNumber.Should().Be(request.EmployeeNumber);
        }
    }
}
