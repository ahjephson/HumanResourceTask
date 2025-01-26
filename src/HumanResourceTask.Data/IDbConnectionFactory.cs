using System.Data;

namespace HumanResourceTask.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
    }
}
