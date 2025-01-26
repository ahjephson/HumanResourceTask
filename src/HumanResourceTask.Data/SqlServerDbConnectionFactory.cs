using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace HumanResourceTask.Data
{
    public class SqlServerDbConnectionFactory : IDbConnectionFactory
    {
        private readonly IOptions<DbConnectionOptions> _options;

        public SqlServerDbConnectionFactory(IOptions<DbConnectionOptions> options)
        {
            _options = options;
        }

        public IDbConnection Create()
        {
            return new SqlConnection(_options.Value.ConnectionString);
        }
    }
}
