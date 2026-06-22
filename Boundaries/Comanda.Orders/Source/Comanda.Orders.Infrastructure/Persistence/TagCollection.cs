namespace Comanda.Orders.Infrastructure.Persistence;

public sealed class TagCollection(IMongoDatabase database) :
    AggregateCollection<Domain.Aggregates.Tag>(database, Collections.Tags),
    ITagCollection
{
    public async Task<IReadOnlyCollection<Domain.Aggregates.Tag>> FilterTagsAsync(TagFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Domain.Aggregates.Tag>()
            .As<Domain.Aggregates.Tag, Domain.Aggregates.Tag, BsonDocument>()
            .FilterTags(filters)
            .Paginate(filters.Pagination)
            .Sort(filters.Sort);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var tags = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<Domain.Aggregates.Tag>(bson))
            .ToList();

        return tags;
    }

    public async Task<System.Numerics.BigInteger> CountTagsAsync(TagFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Domain.Aggregates.Tag>()
            .As<Domain.Aggregates.Tag, Domain.Aggregates.Tag, BsonDocument>()
            .FilterTags(filters)
            .Count();

        var aggregation = await _collection.AggregateAsync(pipeline, cancellationToken: cancellation);
        var result = await aggregation.FirstOrDefaultAsync(cancellation);

        return result?.Count ?? 0;
    }
}
