namespace Comanda.Profiles.Infrastructure.Pipelines;

public static class OwnerFiltersPipelineDefinition
{
    public static PipelineDefinition<Owner, BsonDocument> FilterOwners(
        this PipelineDefinition<Owner, BsonDocument> pipeline, OwnerFilters filters)
    {
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Owner.Identifier, filters.Id),
            FilterDefinitions.MatchIfNotEmpty(Documents.Owner.UserId, filters.UserId),
            FilterDefinitions.MatchIfNotEmpty(Documents.Owner.Email, filters.Email),
            FilterDefinitions.MatchIfNotEmpty(Documents.Owner.PhoneNumber, filters.PhoneNumber),
            FilterDefinitions.MatchBool(Documents.Owner.IsDeleted, filters.IsDeleted),
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
