using System.Data;

namespace Ambev.DeveloperEvaluation.ORM.Factories;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
