using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Ambev.DeveloperEvaluation.ORM.Factories;

public sealed class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public IDbConnection CreateConnection()
        => new NpgsqlConnection(_connectionString);
}
