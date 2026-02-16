namespace Comanda.Stores.Infrastructure.Persistence;

public sealed class ProductCollection(IMongoDatabase database) :
    AggregateCollection<Product>(database, Collections.Products),
    IProductCollection
{
    public async Task<IReadOnlyCollection<Product>> FilterProductsAsync(
        ProductFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Product>()
            .As<Product, Product, BsonDocument>()
            .FilterProducts(filters)
            .Paginate(filters.Pagination)
            .Sort(filters.Sort);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var products = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<Product>(bson))
            .ToList();

        return products;
    }

    public async Task<long> CountProductsAsync(
        ProductFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Product>()
            .As<Product, Product, BsonDocument>()
            .FilterProducts(filters)
            .Count();

        var aggregation = await _collection.AggregateAsync(pipeline, cancellationToken: cancellation);
        var result = await aggregation.FirstOrDefaultAsync(cancellation);

        return result?.Count ?? 0;
    }
}
