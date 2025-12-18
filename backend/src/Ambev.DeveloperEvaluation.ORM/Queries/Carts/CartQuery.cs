using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Queries.Carts;
using Ambev.DeveloperEvaluation.Domain.ReadModels.Carts;
using Ambev.DeveloperEvaluation.ORM.Factories;
using Dapper;
using System.Data;
using System.Text;

namespace Ambev.DeveloperEvaluation.ORM.Queries.Carts;

public class CartQuery : ICartQuery
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CartQuery(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<PaginatedResponse<CartReadModel>> SearchAsync(
    CartSearchFilter filter,
    CancellationToken ct)
    {
        var where = new StringBuilder("WHERE 1 = 1");
        var parameters = new DynamicParameters();

        if (filter.CustomerId.HasValue)
        {
            where.Append(" AND c.\"CustomerId\" = @CustomerId");
            parameters.Add("CustomerId", filter.CustomerId);
        }

        if (filter.BranchId.HasValue)
        {
            where.Append(" AND c.\"BranchId\" = @BranchId");
            parameters.Add("BranchId", filter.BranchId);
        }

        if (filter.Status.HasValue)
        {
            where.Append(" AND c.\"Status\" = @Status");
            parameters.Add("Status", filter.Status.ToString());
        }

        var orderBy = string.IsNullOrWhiteSpace(filter.Order)
            ? "c.\"CreatedAt\" DESC"
            : filter.Order;

        var offset = (filter.Page - 1) * filter.PageSize;
        parameters.Add("Limit", filter.PageSize);
        parameters.Add("Offset", offset);

        var dataSql = $"""
            SELECT
                c."Id",
                c."SaleNumber",
                c."SaleDate",
                c."CustomerId",
                c."TotalAmount",
                c."BranchId",
                b."Name" AS "BranchName",
                c."Status",
                (c."Status" = 'Canceled') AS "IsCanceled",
                c."CreatedAt",
                c."UpdatedAt",
                c."CanceledAt",
                c."CancellationReason"
            FROM "Carts" c
            INNER JOIN "Branches" b ON b."Id" = c."BranchId"
            {where}
            ORDER BY {orderBy}
            LIMIT @Limit OFFSET @Offset
        """;

            var countSql = $"""
            SELECT COUNT(1)
            FROM "Carts" c
            {where}
        """;

        using var conn = _connectionFactory.CreateConnection();
        conn.Open();

        var items = (await conn.QueryAsync<CartReadModel>(dataSql, parameters)).AsList();
        var totalCount = await conn.ExecuteScalarAsync<int>(countSql, parameters);

        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

        return new PaginatedResponse<CartReadModel>(
            items,
            filter.Page,
            totalPages,
            totalCount
        );
    }


    public async Task<CartReadModel?> GetByIdAsync(Guid cartId, CancellationToken ct)
    {
        const string sql = """
            SELECT 
                c."Id",
                c."SaleNumber",
                c."SaleDate",
                c."CustomerId",
                c."TotalAmount",
                c."BranchId",
                b."Name" AS "BranchName",
                c."Status",
                (c."Status" = 'Canceled') AS "IsCanceled",
                c."CreatedAt",
                c."UpdatedAt",
                c."CanceledAt",
                c."CancellationReason"
            FROM "Carts" c
            INNER JOIN "Branches" b ON b."Id" = c."BranchId"
            WHERE c."Id" = @cartId
        """;

        using var conn = _connectionFactory.CreateConnection();
        conn.Open();

        return await conn.QueryFirstOrDefaultAsync<CartReadModel>(sql, new { cartId });
    }

    public async Task<IReadOnlyList<CartProductReadModel>> GetItemsAsync(Guid cartId, CancellationToken ct)
    {
        const string sql = """
            SELECT
                ci."ProductId",
                ci."Quantity"
            FROM "CartItems" ci
            WHERE ci."CartId" = @cartId
            ORDER BY ci."CreatedAt"
        """;

        using var conn = _connectionFactory.CreateConnection();
        conn.Open();

        var items = await conn.QueryAsync<CartProductReadModel>(sql, new { cartId });
        return items.AsList();
    }
}
