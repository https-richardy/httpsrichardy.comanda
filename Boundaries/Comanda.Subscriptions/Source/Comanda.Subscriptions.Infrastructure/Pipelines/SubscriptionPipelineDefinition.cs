namespace Comanda.Subscriptions.Infrastructure.Pipelines;

public static class SubscriptionPipelineDefinition
{
    public static PipelineDefinition<Subscription, BsonDocument> FilterSubscriptions(
        this PipelineDefinition<Subscription, BsonDocument> pipeline, SubscriptionFilters filters)
    {
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Subscription.Identifier, filters.Id),
            FilterDefinitions.MatchIfNotEmpty(Documents.Subscription.SubscriberId, filters.SubscriberId),
            FilterDefinitions.MatchIfNotEmpty(Documents.Subscription.ExternalId, filters.ExternalId),

            FilterDefinitions.MatchIfNotEmptyEnum(Documents.Subscription.Plan, filters.Plan),
            FilterDefinitions.MatchIfNotEmptyEnum(Documents.Subscription.Status, filters.Status)
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
