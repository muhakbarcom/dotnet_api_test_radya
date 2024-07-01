using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Data;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using Npgsql;
using MySql.Data.MySqlClient;
using Microsoft.Data.Sqlite;

public static class DatabaseConfig
{
    public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Load environment variables from .env file
        Env.Load();

        // Retrieve each part of the connection string from environment variables
        var dbType = Environment.GetEnvironmentVariable("DB_TYPE");
        if (string.IsNullOrEmpty(dbType)) throw new ArgumentNullException(nameof(dbType), "Database type (DB_TYPE) must be provided in environment variables.");

        string connectionString = generateConnectionString(dbType);

        // Ensure the database exists
        EnsureDatabaseExists(connectionString, dbType);

        // Configure DbContext based on the database type
        switch (dbType.ToLower())
        {
            case "sqlserver":
                services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));
                break;
            case "postgresql":
                services.AddDbContext<ApplicationDBContext>(options => options.UseNpgsql(connectionString));
                break;
            case "mysql":
                services.AddDbContext<ApplicationDBContext>(options => options.UseMySQL(connectionString));
                break;
            case "sqlite":
                services.AddDbContext<ApplicationDBContext>(options => options.UseSqlite(connectionString));
                break;
            default:
                throw new Exception("Unsupported database type");
        }

    }

    public static bool TestDatabaseConnection(string connectionString, string dbType)
    {
        try
        {
            using (var connection = CreateDbConnection(connectionString, dbType))
            {
                connection.Open();
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to the database: {ex.Message}");
            return false;
        }
    }

    private static DbConnection CreateDbConnection(string connectionString, string dbType)
    {
        switch (dbType.ToLower())
        {
            case "sqlserver":
                return new SqlConnection(connectionString);
            case "postgresql":
                return new NpgsqlConnection(connectionString);
            case "mysql":
                return new MySqlConnection(connectionString);
            case "sqlite":
                return new SqliteConnection(connectionString);
            default:
                throw new Exception("Unsupported database type");
        }
    }

    public static void EnsureDatabaseExists(string connectionString, string dbType)
    {
        switch (dbType.ToLower())
        {
            case "sqlserver":
                CreateSqlServerDatabaseIfNotExists(connectionString);
                break;
            case "postgresql":
                CreatePostgreSqlDatabaseIfNotExists(connectionString);
                break;
            case "mysql":
                CreateMySqlDatabaseIfNotExists(connectionString);
                break;
            case "sqlite":
                CreateSqliteDatabaseIfNotExists(connectionString);
                break;
            default:
                throw new Exception("Unsupported database type");
        }
    }

    private static void CreateSqlServerDatabaseIfNotExists(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var originalDatabase = builder.InitialCatalog;
        builder.InitialCatalog = "master";

        using (var connection = new SqlConnection(builder.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"IF DB_ID('{originalDatabase}') IS NULL CREATE DATABASE [{originalDatabase}]";
                command.ExecuteNonQuery();
            }
        }
    }

    private static void CreatePostgreSqlDatabaseIfNotExists(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var originalDatabase = builder.Database;
        builder.Database = "postgres";

        using (var connection = new NpgsqlConnection(builder.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT 1 FROM pg_database WHERE datname = '{originalDatabase}'";
                var exists = command.ExecuteScalar() != null;

                if (!exists)
                {
                    command.CommandText = $"CREATE DATABASE \"{originalDatabase}\"";
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    private static void CreateMySqlDatabaseIfNotExists(string connectionString)
    {
        var builder = new MySqlConnectionStringBuilder(connectionString);
        var originalDatabase = builder.Database;
        builder.Database = "";


        using (var connection = new MySqlConnection(builder.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"CREATE DATABASE IF NOT EXISTS `{originalDatabase}`";
                command.ExecuteNonQuery();
            }
        }
    }

    private static void CreateSqliteDatabaseIfNotExists(string connectionString)
    {
        // Cek apakah database file sudah ada
        var databaseFilePath = Environment.GetEnvironmentVariable("DB_SERVER") ?? "app.db";

        // Jika file database belum ada, maka buat file database
        if (!File.Exists(databaseFilePath)) File.Create(databaseFilePath).Close();

        // Buka koneksi untuk membuat file database jika belum ada
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            Console.WriteLine($"SQLite database created automatically at {databaseFilePath}");
        }
    }


    private static string generateConnectionString(string dbType)
    {
        // Retrieve each part of the connection string from environment variables
        var server = Environment.GetEnvironmentVariable("DB_SERVER");
        var database = Environment.GetEnvironmentVariable("DB_DATABASE");
        var user = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        var integratedSecurity = Environment.GetEnvironmentVariable("DB_INTEGRATED_SECURITY");
        var connectTimeout = Environment.GetEnvironmentVariable("DB_CONNECT_TIMEOUT");
        var encrypt = Environment.GetEnvironmentVariable("DB_ENCRYPT");
        var trustServerCertificate = Environment.GetEnvironmentVariable("DB_TRUST_SERVER_CERTIFICATE");
        var applicationIntent = Environment.GetEnvironmentVariable("DB_APPLICATION_INTENT");
        var multiSubnetFailover = Environment.GetEnvironmentVariable("DB_MULTI_SUBNET_FAILOVER");

        // Form the connection string
        string connectionString;
        string userPassword = string.Empty;
        if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password)) userPassword = $"User ID={user};Password={password};";

        switch (dbType.ToLower())
        {
            case "sqlserver":
                connectionString = $"Data Source={server};Initial Catalog={database};Integrated Security={integratedSecurity};{userPassword}Connect Timeout={connectTimeout};Encrypt={encrypt};TrustServerCertificate={trustServerCertificate};ApplicationIntent={applicationIntent};MultiSubnetFailover={multiSubnetFailover}";
                break;
            case "postgresql":
                connectionString = $"Host={server};Database={database};Username={user};Password={password}";
                break;
            case "mysql":
                connectionString = $"Server={server};Database={database};User={user};Password={password}";
                break;
            case "sqlite":
                connectionString = $"Data Source={server ?? "app.db"}";
                break;
            default:
                throw new Exception("Unsupported database type");
        }

        return connectionString;
    }
}
