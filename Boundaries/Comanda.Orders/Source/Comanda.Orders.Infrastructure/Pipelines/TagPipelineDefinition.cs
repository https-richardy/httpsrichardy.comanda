namespace Comanda.Orders.Infrastructure.Pipelines;

public static class TagPipelineDefinition
{
    public static PipelineDefinition<Domain.Aggregates.Tag, BsonDocument> FilterTags(
        this PipelineDefinition<Domain.Aggregates.Tag, BsonDocument> pipeline, TagFilters filters)
    {
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Tags.Identifier, filters.Id),
            FilterDefinitions.MatchIfNotEmpty(Documents.Tags.Code, filters.Code),
            FilterDefinitions.MatchIfNotEmpty(Documents.Tags.Merchant, filters.MerchantId)
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
