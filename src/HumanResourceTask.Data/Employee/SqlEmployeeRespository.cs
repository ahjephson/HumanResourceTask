using HumanResourceTask.Data.Employee.Queries;
using HumanResourceTask.Exceptions;
using HumanResourceTask.MetaModels;
using HumanResourceTask.Models;
using HumanResourceTask.Repositories;
using Microsoft.Data.SqlClient;

namespace HumanResourceTask.Data.Employee
{
    public class SqlEmployeeRespository : IEmployeeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDapperWrapper _dapperWrapper;

        public SqlEmployeeRespository(IDbConnectionFactory connectionFactory, IDapperWrapper dapperWrapper)
        {
            _connectionFactory = connectionFactory;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<EmployeeRecordView> CreateEmployeeAsync(EmployeeRecord employeeRecord)
        {
            using (var connection = _connectionFactory.Create())
            {
                try
                {
                    var rowsAffected = await _dapperWrapper.ExecuteAsync(connection, QueriesResource.Insert, employeeRecord);

                    if (rowsAffected < 1)
                    {
                        throw new RepositoryException("Failed to insert the employee record.");
                    }

                    var record = await FindEmployeeAsync(employeeRecord.Id);
                    if (record is null)
                    {
                        throw new RepositoryException("The inserted employee record could not be retrieved.");
                    }
                    return record;
                }
                catch (SqlException exception)
                {
                    throw RepositoryExceptionFactory.FromSqlException(exception);
                }
            }
        }

        public async Task<EmployeeRecordView?> FindEmployeeAsync(Guid id)
        {
            using (var connection = _connectionFactory.Create())
            {
                try
                {
                    return await _dapperWrapper.QueryFirstOrDefaultAsync<EmployeeRecordView>(connection, QueriesResource.FindById, new { Id = id });
                }
                catch (SqlException ex)
                {
                    throw RepositoryExceptionFactory.FromSqlException(ex);
                }
            }
        }

        public async Task<IEnumerable<EmployeeRecordView>> ListEmployeeRecords(
            SortDefinition sortDefinition,
            PaginationDefinition paginationDefinition,
            Guid? statusId = null,
            Guid? departmentId = null)
        {
            using (var connection = _connectionFactory.Create())
            {
                try
                {
                    var parameters = new
                    {
                        StatusId = statusId,
                        DepartmentId = departmentId,
                        OrderBy = sortDefinition.ToString(),
                        paginationDefinition.Offset,
                        paginationDefinition.Limit,
                    };

                    return await _dapperWrapper.QueryAsync<EmployeeRecordView>(connection, QueriesResource.List, parameters);
                }
                catch (SqlException ex)
                {
                    throw RepositoryExceptionFactory.FromSqlException(ex);
                }
            }
        }

        public async Task<long> CountEmployeeRecords(
            Guid? statusId = null,
            Guid? departmentId = null)
        {
            using (var connection = _connectionFactory.Create())
            {
                try
                {
                    var parameters = new
                    {
                        StatusId = statusId,
                        DepartmentId = departmentId,
                    };

                    return await _dapperWrapper.ExecuteScalarAsync<long>(connection, QueriesResource.Count, parameters);
                }
                catch (SqlException ex)
                {
                    throw RepositoryExceptionFactory.FromSqlException(ex);
                }
            }
        }

        public async Task<EmployeeRecordView> UpdateEmployeeAsync(EmployeeRecord employeeRecord)
        {
            using (var connection = _connectionFactory.Create())
            {
                try
                {
                    var rowsAffected = await _dapperWrapper.ExecuteAsync(connection, QueriesResource.Update, employeeRecord);

                    if (rowsAffected < 1)
                    {
                        throw new RepositoryException("Failed to update the employee record.");
                    }

                    var record = await FindEmployeeAsync(employeeRecord.Id);
                    if (record is null)
                    {
                        throw new RepositoryException("The updated employee record could not be retrieved.");
                    }

                    return record;
                }
                catch (SqlException ex)
                {
                    throw RepositoryExceptionFactory.FromSqlException(ex);
                }
            }
        }
    }
}
