using System.Net.Http.Json;
using FluentResults;
using Humanizer;
using HumanResourceTask.Api.Dto.Department;
using HumanResourceTask.Api.Dto.Employee;
using HumanResourceTask.Api.Dto.Status;
using Microsoft.AspNetCore.Http.Extensions;

namespace HumanResourceTask.Web.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<EmployeeResponse>> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync($"employee", request);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var error = await responseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
                return Result.Fail(error);
            }
            var response = await responseMessage.Content.ReadFromJsonAsync<EmployeeResponse>();
            return response ?? throw new InvalidOperationException("Deserialization of EmployeeResponse failed.");
        }

        public async Task<Result> DeleteEmployeeAsync(Guid id)
        {
            var responseMessage = await _httpClient.DeleteAsync($"employee/{id}");
            if (!responseMessage.IsSuccessStatusCode)
            {
                var error = await responseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
                return Result.Fail(error);
            }

            return Result.Ok();
        }

        public async Task<Result<IEnumerable<DepartmentListItem>>> GetDepartmentsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ListDepartmentsResponse>($"departments");
            return Result.Ok<IEnumerable<DepartmentListItem>>(response?.Items ?? []);
        }

        public async Task<Result<EmployeeResponse>> GetEmployeeAsync(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<EmployeeResponse>($"employee/{id}");
            return response ?? throw new InvalidOperationException();
        }

        public string GetFriendlyError(ErrorResponse errorResponse)
        {
            var firstError = errorResponse.Errors.FirstOrDefault();
            return firstError.Key is null ? "An error occurred" : $"{firstError.Key.Humanize()} {string.Join(", ", firstError.Value.Humanize(f => f.Length == 0 ? f : char.ToLower(f[0]) + f[1..]))}";
        }

        public async Task<Result<IEnumerable<StatusListItem>>> GetStatusesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ListStatusesResponse>($"statuses");
            return Result.Ok<IEnumerable<StatusListItem>>(response?.Items ?? []);
        }

        public async Task<Result<ListEmployeesResponse>> ListEmployeesAsync(ListEmployeesRequest request)
        {
            var query = new QueryBuilder();
            if (request.Offset is not null)
            {
                query.Add("offset", request.Offset.Value.ToString());
            }
            if (request.Limit is not null)
            {
                query.Add("limit", request.Limit.Value.ToString());
            }
            if (request.Sort is not null)
            {
                query.Add("sort", $"{(request.Sort.Ascending ? "+" : "-")}{request.Sort.Column}");
            }
            if (request.DepartmentId is not null)
            {
                query.Add("departmentId", request.DepartmentId.Value.ToString());
            }
            if (request.StatusId is not null)
            {
                query.Add("statusId", request.StatusId.Value.ToString());
            }

            var response = await _httpClient.GetFromJsonAsync<ListEmployeesResponse>($"employees{query}");
            return response ?? new ListEmployeesResponse();
        }

        public async Task<Result<EmployeeResponse>> UpdateEmployeeAsync(UpdateEmployeeRequest request)
        {
            var responseMessage = await _httpClient.PatchAsJsonAsync($"employee/{request.Id}", request);
            if (!responseMessage.IsSuccessStatusCode)
            {
                var error = await responseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
                return Result.Fail(error);
            }
            var response = await responseMessage.Content.ReadFromJsonAsync<EmployeeResponse>();
            return response ?? throw new InvalidOperationException("Deserialization of EmployeeResponse failed.");
        }
    }
}
