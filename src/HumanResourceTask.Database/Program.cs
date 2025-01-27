using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DbUp;

namespace HumanResourceTask.Database
{
    internal class Program
    {
        [ExcludeFromCodeCoverage]
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Missing connection string.");
                Environment.Exit(-2);
            }

            var connectionString = args[0];

            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();
            if (result.Successful)
            {
                Console.WriteLine("Database migration completed successfully!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Database migration failed.");
                Console.WriteLine($"Error message: {result.Error.Message}");
                if (result.ErrorScript is not null)
                {
                    Console.WriteLine($"Error in script: {result.ErrorScript.Name}");
                }
                Console.ResetColor();
                Environment.Exit(-1);
            }
        }
    }
}
