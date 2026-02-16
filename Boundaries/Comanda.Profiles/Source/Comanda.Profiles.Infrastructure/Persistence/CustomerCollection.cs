namespace Comanda.Profiles.Infrastructure.Persistence;

public sealed class CustomerCollection(IMongoDatabase database) :
    AggregateCollection<Customer>(database, Collections.Customers),
    ICustomerCollection
{
    public async Task<IReadOnlyCollection<Customer>> GetCustomersAsync(
        CustomerFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Customer>()
            .As<Customer, Customer, BsonDocument>()
            .FilterCustomers(filters)
            .Paginate(filters.Pagination)
            .Sort(filters.Sort);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var customers = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<Customer>(bson))
            .ToList();

        return customers;
    }

    public async Task<long> CountCustomersAsync(
        CustomerFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Customer>()
            .As<Customer, Customer, BsonDocument>()
            .FilterCustomers(filters)
            .Count();

        var aggregation = await _collection.AggregateAsync(pipeline, cancellationToken: cancellation);
        var result = await aggregation.FirstOrDefaultAsync(cancellation);

        return result?.Count ?? 0;
    }
}
