using Marten.Pagination;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>()
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        await Console.Out.WriteLineAsync($"{products.IsFirstPage}");
        await Console.Out.WriteLineAsync($"{products.HasNextPage}");
        await Console.Out.WriteLineAsync($"{products.HasPreviousPage}");
        await Console.Out.WriteLineAsync($"{products.PageCount}");


        return new GetProductsResult(products);
    }
}