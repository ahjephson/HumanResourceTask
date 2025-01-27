using System.Data;
using System.Diagnostics.CodeAnalysis;
using Dapper;
using static Dapper.SqlMapper;

namespace HumanResourceTask.Data
{
    [ExcludeFromCodeCoverage(Justification = "Any tests on this class would onyl be testing Dapper call redirects. Due to the extension method setup Dapper uses this is difficult and not worth the time.")]
    public class DapperWrapper : IDapperWrapper
    {
        public Task<int> ExecuteAsync(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T?> ExecuteScalarAsync<T>(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> QueryFirstAsync<T>(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> QuerySingleAsync<T>(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T?> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T?> QuerySingleOrDefaultAsync<T>(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
