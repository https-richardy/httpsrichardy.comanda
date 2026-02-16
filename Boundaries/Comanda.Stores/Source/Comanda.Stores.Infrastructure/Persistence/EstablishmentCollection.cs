namespace Comanda.Stores.Infrastructure.Persistence;

public sealed class EstablishmentCollection(IMongoDatabase database) :
    AggregateCollection<Establishment>(database, Collections.Establishments),
    IEstablishmentCollection
{
    public async Task<IReadOnlyCollection<Establishment>> FilterEstablishmentsAsync(
        EstablishmentFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Establishment>()
            .As<Establishment, Establishment, BsonDocument>()
            .FilterEstablishments(filters)
            .Paginate(filters.Pagination)
            .Sort(filters.Sort);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var establishments = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<Establishment>(bson))
            .ToList();

        return establishments;
    }

    public async Task<long> CountEstablishmentsAsync(
        EstablishmentFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Establishment>()
            .As<Establishment, Establishment, BsonDocument>()
            .FilterEstablishments(filters)
            .Count();

        var aggregation = await _collection.AggregateAsync(pipeline, cancellationToken: cancellation);
        var result = await aggregation.FirstOrDefaultAsync(cancellation);

        return result?.Count ?? 0;
    }
}
