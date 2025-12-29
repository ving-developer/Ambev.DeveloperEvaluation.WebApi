using Ambev.DeveloperEvaluation.ORM.Factories;
using Npgsql;
using System.Data;

namespace Ambev.DeveloperEvaluation.Integration.Shared.Helpers;

/// <summary>
/// Local test connection factory that uses the Testcontainer connection string.
/// This guarantees Dapper (IDbConnectionFactory) opens the same DB used by EF migrations/seeding.
/// </summary>
internal sealed class TestNpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public TestNpgsqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}
