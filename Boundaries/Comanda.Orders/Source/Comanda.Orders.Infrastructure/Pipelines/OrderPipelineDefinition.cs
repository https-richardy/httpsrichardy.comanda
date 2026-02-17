namespace Comanda.Orders.Infrastructure.Pipelines;

public static class OrderPipelineDefinition
{
    public static PipelineDefinition<Order, BsonDocument> FilterOrders(
        this PipelineDefinition<Order, BsonDocument> pipeline, OrderFilters filters)
    {
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Orders.Identifier, filters.Id),
            FilterDefinitions.MatchIfNotEmpty(Documents.Orders.Code, filters.Code),
            FilterDefinitions.MatchIfNotEmpty(Documents.Orders.Merchant, filters.MerchantId),
            FilterDefinitions.MatchIfNotEmpty(Documents.Orders.Consumer, filters.ConsumerId),

            FilterDefinitions.MatchIfNotEmptyEnum(Documents.Orders.Status, filters.Status),
            FilterDefinitions.MatchIfNotEmptyEnum(Documents.Orders.Priority, filters.Priority),
            FilterDefinitions.MatchIfNotEmptyEnum(Documents.Orders.Fulfillment, filters.Fulfillment),

            FilterDefinitions.MustBeWithinIfNotNull(Documents.Orders.CreatedAt, filters.CreatedAfter, filters.CreatedBefore),
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
