using HumanResourceTask.Data.Status.Queries;
using HumanResourceTask.Repositories;
using Microsoft.Data.SqlClient;

namespace HumanResourceTask.Data.Status
{
    public class SqlStatusRepository : IStatusRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDapperWrapper _dapperWrapper;

        public SqlStatusRepository(IDbConnectionFactory connectionFactory, IDapperWrapper dapperWrapper)
        {
            _connectionFactory = connectionFactory;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<IEnumerable<Models.Status>> ListStatusesAsync()
        {
            using (var connection = _connectionFactory.Create())
            {
                try
                {
                    return await _dapperWrapper.QueryAsync<Models.Status>(connection, QueriesResource.List);
                }
                catch (SqlException ex)
                {
                    throw RepositoryExceptionFactory.FromSqlException(ex);
                }
            }
        }
    }
}
