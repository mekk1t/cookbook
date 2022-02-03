using Npgsql;
using SimpleMigrations;
using SimpleMigrations.Console;
using SimpleMigrations.DatabaseProvider;

namespace KP.Cookbook.Migrations
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("Не указаны аргументы командной строки");

            var connectionString = args[0];
            using var connection = new NpgsqlConnection(connectionString);

            var databaseProvider = new PostgresqlDatabaseProvider(connection);
            var migrator = new SimpleMigrator(typeof(Program).Assembly, databaseProvider);

            migrator.Load();

            var consoleRunner = new ConsoleRunner(migrator);

            return consoleRunner.Run(args);
        }
    }
}
