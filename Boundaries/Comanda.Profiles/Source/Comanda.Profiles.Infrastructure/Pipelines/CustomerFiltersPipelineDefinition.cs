namespace Comanda.Profiles.Infrastructure.Pipelines;

public static class CustomerFiltersPipelineDefinition
{
    public static PipelineDefinition<Customer, BsonDocument> FilterCustomers(
        this PipelineDefinition<Customer, BsonDocument> pipeline, CustomerFilters filters)
    {
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Customer.Identifier, filters.Id),
            FilterDefinitions.MatchIfNotEmpty(Documents.Customer.UserId, filters.UserId),
            FilterDefinitions.MatchIfNotEmpty(Documents.Customer.Email, filters.Email),
            FilterDefinitions.MatchIfNotEmpty(Documents.Customer.PhoneNumber, filters.PhoneNumber),
            FilterDefinitions.MatchBool(Documents.Customer.IsDeleted, filters.IsDeleted)
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
