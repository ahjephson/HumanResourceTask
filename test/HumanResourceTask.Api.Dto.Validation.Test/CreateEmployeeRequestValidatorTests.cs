using FluentValidation.TestHelper;
using HumanResourceTask.Api.Dto.Employee;

namespace HumanResourceTask.Api.Dto.Validation.Test
{
    public class CreateEmployeeRequestValidatorTests
    {
        private readonly CreateEmployeeRequestValidator _target = new CreateEmployeeRequestValidator();

        [Fact]
        public void GIVEN_FirstNameIsNull_WHEN_ValidatingRequestObject_THEN_ShouldFailValidationWithMessage()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = null!,
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345
            };

            var result = _target.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("First name must be provided.");
        }

        [Fact]
        public void GIVEN_FirstNameIsEmpty_WHEN_ValidatingRequestObject_THEN_ShouldFailValidationWithMessage()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = string.Empty,
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345
            };

            var result = _target.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("First name must be provided.");
        }

        [Fact]
        public void GIVEN_LastNameIsNull_WHEN_ValidatingRequestObject_THEN_ShouldFailValidationWithMessage()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "FirstName",
                LastName = null!,
                Email = "Email",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345
            };

            var result = _target.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("Last name must be provided.");
        }

        [Fact]
        public void GIVEN_LastNameIsEmpty_WHEN_ValidatingRequestObject_THEN_ShouldFailValidationWithMessage()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "FirstName",
                LastName = string.Empty,
                Email = "Email",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345
            };

            var result = _target.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("Last name must be provided.");
        }

        [Fact]
        public void GIVEN_EmailIsNull_WHEN_ValidatingRequestObject_THEN_ShouldFailValidationWithMessage()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = null!,
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345
            };

            var result = _target.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email must be provided.");
        }

        [Fact]
        public void GIVEN_EmailIsEmpty_WHEN_ValidatingRequestObject_THEN_ShouldFailValidationWithMessage()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = string.Empty,
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345
            };

            var result = _target.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email must be provided.");
        }

        [Fact]
        public void GIVEN_EmailIsNotAnEmailAddress_WHEN_ValidatingRequestObject_THEN_ShouldFailValidationWithMessage()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "NotAnEmailAddress",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345
            };

            var result = _target.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email must be a valid email address.");
        }

        [Fact]
        public void GIVEN_DateOfBirthIsTooOld_WHEN_ValidatingRequestObject_THEN_ShouldFailValidationWithMessage()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-151)),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345
            };

            var result = _target.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
                .WithErrorMessage("Date of birth must not be more than 150 years ago.");
        }

        [Fact]
        public void GIVEN_DateOfBirthIsTooYoung_WHEN_ValidatingRequestObject_THEN_ShouldFailValidationWithMessage()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-15)),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345
            };

            var result = _target.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
                .WithErrorMessage("Employee must be at least 16 years old.");
        }

        [Fact]
        public void GIVEN_DepartmentIdIsEmpty_WHEN_ValidatingRequestObject_THEN_ShouldFailValidationWithMessage()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                DepartmentId = Guid.Empty,
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 12345
            };

            var result = _target.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.DepartmentId)
                .WithErrorMessage("Department must be provided.");
        }

        [Fact]
        public void GIVEN_StatusIdIsEmpty_WHEN_ValidatingRequestObject_THEN_ShouldFailValidationWithMessage()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.Empty,
                EmployeeNumber = 12345
            };

            var result = _target.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.StatusId)
                .WithErrorMessage("Status must be provided.");
        }

        [Fact]
        public void GIVEN_EmployeeNumberIsNotPositive_WHEN_ValidatingRequestObject_THEN_ShouldFailValidationWithMessage()
        {
            var request = new CreateEmployeeRequest
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                DepartmentId = Guid.NewGuid(),
                StatusId = Guid.NewGuid(),
                EmployeeNumber = 0
            };

            var result = _target.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.EmployeeNumber)
                .WithErrorMessage("Employee number must be a positive value.");
        }
    }
}
