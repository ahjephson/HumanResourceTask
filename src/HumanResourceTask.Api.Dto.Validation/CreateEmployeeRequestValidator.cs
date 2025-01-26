using FluentValidation;
using FluentValidation.Results;
using HumanResourceTask.Api.Dto.Employee;

namespace HumanResourceTask.Api.Dto.Validation
{
    public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotNull()
                .NotEmpty()
                .WithMessage("First name must be provided.");

            RuleFor(x => x.FirstName)
                .MaximumLength(100)
                .WithMessage("First name must at most 100 characters.");

            RuleFor(x => x.LastName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Last name must be provided.");

            RuleFor(x => x.LastName)
                .MaximumLength(100)
                .WithMessage("Last name must at most 100 characters.");

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("Email must be provided.");

            RuleFor(x => x.Email)
                .MaximumLength(320)
                .WithMessage("Email must at most 320 characters.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email must be a valid email address.");

            RuleFor(x => x.DateOfBirth)
                .NotNull()
                .WithMessage("Date of birth must be provided.");

            RuleFor(x => x.DateOfBirth)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.AddYears(-150)))
                .WithMessage("Date of birth must not be more than 150 years ago.");

            RuleFor(x => x.DateOfBirth)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.AddYears(-16)))
                .WithMessage("Employee must be at least 16 years old.");

            RuleFor(x => x.DepartmentId)
                .NotNull()
                .NotEqual(Guid.Empty)
                .WithMessage("Department must be provided.");

            RuleFor(x => x.StatusId)
                .NotNull()
                .NotEqual(Guid.Empty)
                .WithMessage("Status must be provided.");

            RuleFor(x => x.EmployeeNumber)
                .NotNull()
                .WithMessage("Employee number must be provided.");

            RuleFor(x => x.EmployeeNumber)
                .GreaterThan(0)
                .WithMessage("Employee number must be a positive value.");
        }
    }
}
