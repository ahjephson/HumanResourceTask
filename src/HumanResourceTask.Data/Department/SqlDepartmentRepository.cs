using HumanResourceTask.Data.Department.Queries;
using HumanResourceTask.Repositories;
using Microsoft.Data.SqlClient;

namespace HumanResourceTask.Data.Department
{
    public class SqlDepartmentRepository : IDepartmentRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDapperWrapper _dapperWrapper;

        public SqlDepartmentRepository(IDbConnectionFactory connectionFactory, IDapperWrapper dapperWrapper)
        {
            _connectionFactory = connectionFactory;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<IEnumerable<Models.Department>> ListDepartmentsAsync()
        {
            using (var connection = _connectionFactory.Create())
            {
                try
                {
                    return await _dapperWrapper.QueryAsync<Models.Department>(connection, QueriesResource.List);
                }
                catch (SqlException ex)
                {
                    throw RepositoryExceptionFactory.FromSqlException(ex);
                }
            }
        }
    }
}
