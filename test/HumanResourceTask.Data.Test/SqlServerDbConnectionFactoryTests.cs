using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Moq;

namespace HumanResourceTask.Data.Test
{
    public class SqlServerDbConnectionFactoryTests
    {
        [Fact]
        public void GIVEN_ValidOptions_WHEN_Create_THEN_ShouldReturnSqlConnection()
        {
            var connectionString = "Server=localhost;Database=TestDb;Trusted_Connection=True;";
            var dbConnectionOptions = new DbConnectionOptions { ConnectionString = connectionString };

            var optionsMock = new Mock<IOptions<DbConnectionOptions>>();
            optionsMock
                .Setup(o => o.Value)
                .Returns(dbConnectionOptions);

            var factory = new SqlServerDbConnectionFactory(optionsMock.Object);

            using var connection = factory.Create();

            connection.Should().NotBeNull();
            connection.Should().BeOfType<SqlConnection>();
            connection.ConnectionString.Should().Be(connectionString);
        }
    }
}
