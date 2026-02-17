namespace Comanda.Payments.Infrastructure.Persistence;

public sealed class PaymentCollection(IMongoDatabase database) :
    AggregateCollection<Payment>(database, Collections.Payments),
    IPaymentCollection
{
    public async Task<IReadOnlyCollection<Payment>> FilterPaymentsAsync(
        PaymentFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Payment>()
            .As<Payment, Payment, BsonDocument>()
            .FilterPayments(filters)
            .Cursor(filters.Cursor)
            .Sort(filters.Sort);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var payments = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<Payment>(bson))
            .ToList();

        return payments;
    }

    public async Task<long> CountPaymentsAsync(
        PaymentFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Payment>()
            .As<Payment, Payment, BsonDocument>()
            .FilterPayments(filters)
            .Count();

        var aggregation = await _collection.AggregateAsync(pipeline, cancellationToken: cancellation);
        var result = await aggregation.FirstOrDefaultAsync(cancellation);

        return result?.Count ?? 0;
    }
}
