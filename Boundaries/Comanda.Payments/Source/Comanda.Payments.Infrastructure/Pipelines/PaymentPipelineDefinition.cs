namespace Comanda.Payments.Infrastructure.Pipelines;

public static class PaymentPipelineDefinition
{
    public static PipelineDefinition<Payment, BsonDocument> FilterPayments(
        this PipelineDefinition<Payment, BsonDocument> pipeline, PaymentFilters filters)
    {
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Payment.Identifier, filters.Id),
            FilterDefinitions.MatchIfNotEmpty(Documents.Payment.PayerId, filters.PayerId),
            FilterDefinitions.MatchIfNotEmpty(Documents.Payment.ExternalId, filters.ExternalId),
            FilterDefinitions.MatchIfNotEmpty(Documents.Payment.ReferenceId, filters.ReferenceId),

            FilterDefinitions.MatchIfNotEmptyEnum(Documents.Payment.Status, filters.Status),
            FilterDefinitions.MatchIfNotEmptyEnum(Documents.Payment.Method, filters.Method),

            FilterDefinitions.MustBeWithinIfNotNull(Documents.Payment.CreatedAt, filters.CreatedAfter, filters.CreatedBefore),
            FilterDefinitions.MustBeWithinIfNotNull(Documents.Payment.Amount, filters.MinAmount, filters.MaxAmount)
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
