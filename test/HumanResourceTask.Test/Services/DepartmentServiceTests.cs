using FluentAssertions;
using HumanResourceTask.Models;
using HumanResourceTask.Repositories;
using HumanResourceTask.Services;
using Moq;

namespace HumanResourceTask.Test.Services
{
    public class DepartmentServiceTests
    {
        [Fact]
        public async Task GIVEN_DepartmentRepositoryReturnsDepartments_WHEN_GetDepartmentsAsyncIsCalled_THEN_ShouldReturnSuccessResultWithDepartments()
        {
            var mockDepartmentRepository = new Mock<IDepartmentRepository>();
            var departments = new List<Department>
            {
                new Department { Id = Guid.NewGuid(), Name = "Name1" },
                new Department { Id = Guid.NewGuid(), Name = "Name2" }
            };

            mockDepartmentRepository
                .Setup(repo => repo.ListDepartmentsAsync())
                .ReturnsAsync(departments);

            var service = new DepartmentService(mockDepartmentRepository.Object);

            var result = await service.GetDepartmentsAsync();

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(departments);
        }

        [Fact]
        public async Task GIVEN_DepartmentRepositoryThrowsException_WHEN_GetDepartmentsAsyncIsCalled_THEN_ShouldReturnFailureResult()
        {
            var mockDepartmentRepository = new Mock<IDepartmentRepository>();

            mockDepartmentRepository
                .Setup(repo => repo.ListDepartmentsAsync())
                .ThrowsAsync(new Exception("Message"));

            var service = new DepartmentService(mockDepartmentRepository.Object);

            var result = await service.GetDepartmentsAsync();

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(error => error.Message == "Message");
        }
    }
}
