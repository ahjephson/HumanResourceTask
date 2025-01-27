using System.Diagnostics.CodeAnalysis;
using Dapper;
using HumanResourceTask.Data;
using HumanResourceTask.Data.Department;
using HumanResourceTask.Data.Employee;
using HumanResourceTask.Data.Status;
using HumanResourceTask.Repositories;
using HumanResourceTask.Services;

namespace HumanResourceTask.Api.Initialisation
{
    public static class ApplicationInitialiser
    {
        [ExcludeFromCodeCoverage]
        public static void AddApplication(this IServiceCollection services)
        {
            AddResositories(services);
            AddServices(services);
        }

        [ExcludeFromCodeCoverage]
        private static void AddResositories(IServiceCollection services)
        {
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
            SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());

            services.AddSingleton<IDbConnectionFactory, SqlServerDbConnectionFactory>();
            services.AddSingleton<IDapperWrapper, DapperWrapper>();

            services.AddSingleton<IEmployeeRepository, SqlEmployeeRespository>();
            services.AddSingleton<IStatusRepository, SqlStatusRepository>();
            services.AddSingleton<IDepartmentRepository, SqlDepartmentRepository>();
        }

        [ExcludeFromCodeCoverage]
        private static void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddSingleton<IStatusService, StatusService>();
            services.AddSingleton<IDepartmentService, DepartmentService>();
        }
    }
}
