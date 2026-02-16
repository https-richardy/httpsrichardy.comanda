namespace Comanda.Stores.Infrastructure.Pipelines;

public static class EstablishmentPipelineDefinition
{
    public static PipelineDefinition<Establishment, BsonDocument> FilterEstablishments(
        this PipelineDefinition<Establishment, BsonDocument> pipeline, EstablishmentFilters filters)
    {
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Establishment.Identifier, filters.Id),
            FilterDefinitions.MatchIfNotEmpty(Documents.Establishment.OwnerId, filters.OwnerId),
            FilterDefinitions.MatchIfContains(Documents.Establishment.Title, filters.Title),
            FilterDefinitions.MatchBool(Documents.Establishment.IsDeleted, filters.IsDeleted),
            FilterDefinitions.MustBeWithinIfNotNull(Documents.Establishment.CreatedAt, filters.CreatedAfter, filters.CreatedBefore)
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
