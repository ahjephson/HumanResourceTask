using Bunit;
using FluentAssertions;
using Moq;
using HumanResourceTask.Web.Pages;
using HumanResourceTask.Api.Dto.Department;
using HumanResourceTask.Api.Dto.Status;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Web.Services;
using MudBlazor;
using Microsoft.AspNetCore.Components;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;

namespace HumanResourceTask.Web.Test.Pages
{
    public class EmployeeCreateTests : TestMudContext
    {
        private readonly Mock<IApiClient> _mockApiClient;
        private readonly Mock<ISnackbar> _mockSnackbar;
        private readonly Mock<NavigationManager> _mockNavigationManager;

        public EmployeeCreateTests()
        {
            _mockApiClient = new Mock<IApiClient>();
            _mockSnackbar = new Mock<ISnackbar>();
            _mockNavigationManager = new Mock<NavigationManager>();

            Services.AddSingleton(_mockApiClient.Object);
            Services.AddSingleton(_mockSnackbar.Object);
            Services.AddSingleton(_mockNavigationManager.Object);

            _mockApiClient.Setup(x => x.GetDepartmentsAsync())
              .ReturnsAsync(Result.Ok<IEnumerable<DepartmentListItem>>(new List<DepartmentListItem>()));

            _mockApiClient.Setup(x => x.GetStatusesAsync())
                          .ReturnsAsync(Result.Ok<IEnumerable<StatusListItem>>(new List<StatusListItem>()));
        }

        [Fact]
        public void GIVEN_InitialRender_WHEN_PageLoads_THEN_DepartmentsAndStatusesAreFetched()
        {
            var departments = new[] { new DepartmentListItem { Id = Guid.NewGuid(), Name = "Name" } };
            var statuses = new[] { new StatusListItem { Id = Guid.NewGuid(), Name = "Name" } };

            _mockApiClient.Setup(x => x.GetDepartmentsAsync())
                          .ReturnsAsync(Result.Ok<IEnumerable<DepartmentListItem>>(departments));

            _mockApiClient.Setup(x => x.GetStatusesAsync())
                          .ReturnsAsync(Result.Ok<IEnumerable<StatusListItem>>(statuses));

            var component = RenderComponent<EmployeeCreate>();

            component.Instance.Departments.Should().BeEquivalentTo(departments);
            component.Instance.Statuses.Should().BeEquivalentTo(statuses);
            _mockApiClient.Verify(x => x.GetDepartmentsAsync(), Times.Once);
            _mockApiClient.Verify(x => x.GetStatusesAsync(), Times.Once);
        }

        [Fact]
        public async Task GIVEN_InvalidForm_WHEN_SubmitIsClicked_THEN_FormIsNotSubmitted()
        {
            var component = RenderComponent<EmployeeCreate>();

            //var submitButton = component.Find("button.mud-button-filled-primary");
            //submitButton.Click();

            var submitButton = component.FindComponent<MudButton>();
            await component.InvokeAsync(() =>
            {
                submitButton.Instance.OnClick.InvokeAsync(null);
            });

            _mockApiClient.Verify(x => x.CreateEmployeeAsync(It.IsAny<CreateEmployeeRequest>()), Times.Never);
        }

        [Fact]
        public async Task GIVEN_ValidForm_WHEN_SubmitIsClicked_THEN_EmployeeIsCreated()
        {
            var employeeId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();
            var statusId = Guid.NewGuid();

            _mockApiClient.Setup(x => x.CreateEmployeeAsync(It.IsAny<CreateEmployeeRequest>()))
                          .ReturnsAsync(Result.Ok(new EmployeeResponse
                          {
                              Id = employeeId,
                              FirstName = "FirstName",
                              LastName = "LastName",
                              Email = "example@email.com",
                              DepartmentId = departmentId,
                              DepartmentName = "DepartmentName",
                              StatusId = statusId,
                              StatusName = "StatusName",
                              EmployeeNumber = 1234,
                              CreatedAtUtc = DateTimeOffset.UtcNow
                          }));

            _mockApiClient.Setup(x => x.GetDepartmentsAsync())
                          .ReturnsAsync(Result.Ok<IEnumerable<DepartmentListItem>>(new[]
                          {
                              new DepartmentListItem { Id = departmentId, Name = "DepartmentName" }
                          }));

            _mockApiClient.Setup(x => x.GetStatusesAsync())
                          .ReturnsAsync(Result.Ok<IEnumerable<StatusListItem>>(new[]
                          {
                              new StatusListItem { Id = statusId, Name = "StatusName" }
                          }));

            var component = RenderComponent<EmployeeCreate>();

            await component.InvokeAsync(() =>
            {
                SetPrivateProperty(component.Instance, "Model", new CreateEmployeeRequest
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Email = "example@email.com",
                    DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
                    DepartmentId = departmentId,
                    StatusId = statusId,
                    EmployeeNumber = 1234
                });
            });

            component.Render();

            await component.InvokeAsync(() =>
            {
                component.FindComponent<MudButton>().Instance.OnClick.InvokeAsync(null);
            });

            _mockApiClient.Verify(x => x.CreateEmployeeAsync(It.Is<CreateEmployeeRequest>(req =>
                req.FirstName == "FirstName" &&
                req.LastName == "LastName" &&
                req.Email == "example@email.com" &&
                req.DateOfBirth == DateOnly.FromDateTime(DateTime.Today.AddYears(-25)) &&
                req.DepartmentId == departmentId &&
                req.StatusId == statusId &&
                req.EmployeeNumber == 1234
            )), Times.Once);

            _mockSnackbar.Verify(x => x.Add(
                It.Is<string>(s => s == "Employee created."),
                It.Is<Severity>(sev => sev == Severity.Success),
                It.IsAny<Action<SnackbarOptions>>(),
                It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GIVEN_ServerError_WHEN_SubmitIsClicked_THEN_ErrorMessageIsDisplayed()
        {
            var departmentId = Guid.NewGuid();
            var statusId = Guid.NewGuid();

            var errorResponse = new ErrorResponse
            {
                Message = "ValidationError",
            };

            _mockApiClient.Setup(x => x.GetFriendlyError(It.IsAny<ErrorResponse>()))
                          .Returns("Email must be unique.");

            _mockApiClient.Setup(x => x.CreateEmployeeAsync(It.IsAny<CreateEmployeeRequest>()))
                          .ReturnsAsync(Result.Fail(errorResponse));

            _mockApiClient.Setup(x => x.GetDepartmentsAsync())
                          .ReturnsAsync(Result.Ok<IEnumerable<DepartmentListItem>>(new[]
                          {
                              new DepartmentListItem { Id = departmentId, Name = "DepartmentName" }
                          }));

            _mockApiClient.Setup(x => x.GetStatusesAsync())
                          .ReturnsAsync(Result.Ok<IEnumerable<StatusListItem>>(new[]
                          {
                              new StatusListItem { Id = statusId, Name = "StatusName" }
                          }));

            var component = RenderComponent<EmployeeCreate>();

            await component.InvokeAsync(() =>
            {
                SetPrivateProperty(component.Instance, "Model", new CreateEmployeeRequest
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Email = "example@email.com",
                    DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
                    DepartmentId = departmentId,
                    StatusId = statusId,
                    EmployeeNumber = 1234
                });
            });

            component.Render();

            await component.InvokeAsync(() =>
            {
                component.FindComponent<MudButton>().Instance.OnClick.InvokeAsync(null);
            });

            component.FindComponent<MudAlert>().Find(".mud-alert-message").InnerHtml.Should().Be("Email must be unique.");
        }
    }
}
