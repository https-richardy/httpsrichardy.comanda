namespace Comanda.Profiles.Infrastructure.Persistence;

public sealed class OwnerCollection(IMongoDatabase database) :
    AggregateCollection<Owner>(database, Collections.Owners),
    IOwnerCollection
{
    public async Task<IReadOnlyCollection<Owner>> GetOwnersAsync(
        OwnerFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Owner>()
            .As<Owner, Owner, BsonDocument>()
            .FilterOwners(filters)
            .Paginate(filters.Pagination)
            .Sort(filters.Sort);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var owners = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<Owner>(bson))
            .ToList();

        return owners;
    }

    public async Task<long> CountOwnersAsync(
        OwnerFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Owner>()
            .As<Owner, Owner, BsonDocument>()
            .FilterOwners(filters)
            .Count();

        var aggregation = await _collection.AggregateAsync(pipeline, cancellationToken: cancellation);
        var result = await aggregation.FirstOrDefaultAsync(cancellation);

        return result?.Count ?? 0;
    }
}
