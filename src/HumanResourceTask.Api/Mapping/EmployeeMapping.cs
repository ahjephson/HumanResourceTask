using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Models;

namespace HumanResourceTask.Api.Mapping
{
    public static class EmployeeMapping
    {
        public static EmployeeResponse ToResponseDto(this EmployeeRecordView record)
        {
            return new EmployeeResponse
            {
                Id = record.Id,
                FirstName = record.FirstName,
                LastName = record.LastName,
                Email = record.Email,
                DateOfBirth = record.DateOfBirth,
                DepartmentId = record.DepartmentId,
                DepartmentName = record.DepartmentName,
                StatusId = record.StatusId,
                StatusName = record.StatusName,
                EmployeeNumber = record.EmployeeNumber,
                CreatedAtUtc = record.CreatedAtUtc,
                UpdatedAtUtc = record.UpdatedAtUtc,
                Deleted = record.Deleted,
            };
        }

        public static EmployeeListItem ToListItemDto(this EmployeeRecordView record)
        {
            return new EmployeeListItem
            {
                Id = record.Id,
                FirstName = record.FirstName,
                LastName = record.LastName,
                DateOfBirth = record.DateOfBirth,
                DepartmentName = record.DepartmentName,
                StatusName = record.StatusName,
                EmployeeNumber = record.EmployeeNumber,
            };
        }

        public static IEnumerable<EmployeeListItem> ToListItemDtos(this IEnumerable<EmployeeRecordView> records)
        {
            return records.Select(ToListItemDto);
        }

        public static EmployeeRecord ToModel(this UpdateEmployeeRequest request, EmployeeRecord employeeRecord)
        {
            if (request.FirstName is not null)
            {
                employeeRecord = employeeRecord with { FirstName = request.FirstName };
            }
            if (request.LastName is not null)
            {
                employeeRecord = employeeRecord with { LastName = request.LastName };
            }
            if (request.Email is not null)
            {
                employeeRecord = employeeRecord with { Email = request.Email };
            }
            if (request.DateOfBirth is not null)
            {
                employeeRecord = employeeRecord with { DateOfBirth = request.DateOfBirth.Value };
            }
            if (request.DepartmentId is not null)
            {
                employeeRecord = employeeRecord with { DepartmentId = request.DepartmentId.Value };
            }
            if (request.StatusId is not null)
            {
                employeeRecord = employeeRecord with { StatusId = request.StatusId.Value };
            }
            if (request.EmployeeNumber is not null)
            {
                employeeRecord = employeeRecord with { EmployeeNumber = request.EmployeeNumber.Value };
            }

            return employeeRecord;
        }
    }
}
