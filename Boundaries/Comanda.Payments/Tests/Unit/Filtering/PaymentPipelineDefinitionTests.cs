namespace Comanda.Payments.TestSuite.Unit.Filtering;

public sealed class PaymentPipelineDefinitionTests
{
    [Fact(DisplayName = "[filters] - when payment identifier is provided then it should be included in the filter definition")]
    public void When_PaymentIdentifierIsProvided_ThenItShouldBeIncludedInTheFilterDefinition()
    {
        var identifier = Identifier.Generate<Payment>();
        var filters = PaymentFilters.WithSpecifications()
            .WithIdentifier(identifier)
            .Build();

        var pipeline = PipelineDefinitionBuilder
            .For<Payment>()
            .As<Payment, Payment, BsonDocument>()
            .FilterPayments(filters);

        var rendered = pipeline.Render(new(
            BsonSerializer.SerializerRegistry.GetSerializer<Payment>(),
            BsonSerializer.SerializerRegistry));

        Assert.Single(rendered.Documents);

        var match = rendered.Documents[0]["$match"].AsBsonDocument;

        Assert.True(match.Contains(Documents.Payment.Identifier));
        Assert.Equal(identifier, match[Documents.Payment.Identifier].AsString);
    }

    [Fact(DisplayName = "[filters] - when payer identifier is provided then it should be included in the filter definition")]
    public void When_PayerIdentifierIsProvided_ThenItShouldBeIncludedInTheFilterDefinition()
    {
        var identifier = Identifier.Generate<User>();
        var filters = PaymentFilters.WithSpecifications()
            .WithPayerId(identifier)
            .Build();

        var pipeline = PipelineDefinitionBuilder
            .For<Payment>()
            .As<Payment, Payment, BsonDocument>()
            .FilterPayments(filters);

        var rendered = pipeline.Render(new(
            BsonSerializer.SerializerRegistry.GetSerializer<Payment>(),
            BsonSerializer.SerializerRegistry));

        Assert.Single(rendered.Documents);

        var match = rendered.Documents[0]["$match"].AsBsonDocument;

        Assert.True(match.Contains(Documents.Payment.PayerId));
        Assert.Equal(identifier, match[Documents.Payment.PayerId].AsString);
    }

    [Fact(DisplayName = "[filters] - when external identifier is provided then it should be included in the filter definition")]
    public void When_ExternalIdentifierIsProvided_ThenItShouldBeIncludedInTheFilterDefinition()
    {
        var identifier = Identifier.Generate<Payment>();
        var filters = PaymentFilters.WithSpecifications()
            .WithExternalId(identifier)
            .Build();

        var pipeline = PipelineDefinitionBuilder
            .For<Payment>()
            .As<Payment, Payment, BsonDocument>()
            .FilterPayments(filters);

        var rendered = pipeline.Render(new(
            BsonSerializer.SerializerRegistry.GetSerializer<Payment>(),
            BsonSerializer.SerializerRegistry));

        Assert.Single(rendered.Documents);

        var match = rendered.Documents[0]["$match"].AsBsonDocument;

        Assert.True(match.Contains(Documents.Payment.ExternalId));
        Assert.Equal(identifier, match[Documents.Payment.ExternalId].AsString);
    }

    [Fact(DisplayName = "[filters] - when reference identifier is provided then it should be included in the filter definition")]
    public void When_ReferenceIdentifierIsProvided_ThenItShouldBeIncludedInTheFilterDefinition()
    {
        var reference = Identifier.Generate<Payment>();
        var filters = PaymentFilters.WithSpecifications()
            .WithReferenceId(reference)
            .Build();

        var pipeline = PipelineDefinitionBuilder
            .For<Payment>()
            .As<Payment, Payment, BsonDocument>()
            .FilterPayments(filters);

        var rendered = pipeline.Render(new(
            BsonSerializer.SerializerRegistry.GetSerializer<Payment>(),
            BsonSerializer.SerializerRegistry));

        Assert.Single(rendered.Documents);

        var match = rendered.Documents[0]["$match"].AsBsonDocument;

        Assert.True(match.Contains(Documents.Payment.ReferenceId));
        Assert.Equal(reference, match[Documents.Payment.ReferenceId].AsString);
    }

    [Fact(DisplayName = "[filters] - when status is provided then it should be included in the filter definition")]
    public void When_StatusIsProvided_ThenItShouldBeIncludedInTheFilterDefinition()
    {
        var status = Status.Paid;
        var filters = PaymentFilters.WithSpecifications()
            .WithStatus(status)
            .Build();

        var pipeline = PipelineDefinitionBuilder
            .For<Payment>()
            .As<Payment, Payment, BsonDocument>()
            .FilterPayments(filters);

        var rendered = pipeline.Render(new(
            BsonSerializer.SerializerRegistry.GetSerializer<Payment>(),
            BsonSerializer.SerializerRegistry));

        Assert.Single(rendered.Documents);

        var match = rendered.Documents[0]["$match"].AsBsonDocument;

        Assert.True(match.Contains(Documents.Payment.Status));
        Assert.Equal((int)status, match[Documents.Payment.Status].ToInt32());
    }

    [Fact(DisplayName = "[filters] - when method is provided then it should be included in the filter definition")]
    public void When_MethodIsProvided_ThenItShouldBeIncludedInTheFilterDefinition()
    {
        var method = Method.Card;
        var filters = PaymentFilters.WithSpecifications()
            .WithMethod(method)
            .Build();

        var pipeline = PipelineDefinitionBuilder
            .For<Payment>()
            .As<Payment, Payment, BsonDocument>()
            .FilterPayments(filters);

        var rendered = pipeline.Render(new(
            BsonSerializer.SerializerRegistry.GetSerializer<Payment>(),
            BsonSerializer.SerializerRegistry));

        Assert.Single(rendered.Documents);

        var match = rendered.Documents[0]["$match"].AsBsonDocument;

        Assert.True(match.Contains(Documents.Payment.Method));
        Assert.Equal((int)method, match[Documents.Payment.Method].ToInt32());
    }

    [Fact(DisplayName = "[filters] - when creation period is provided then it should be included in the filter definition")]
    public void When_CreationPeriodIsProvided_ThenItShouldBeIncludedInTheFilterDefinition()
    {
        var createdAfter = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2));
        var createdBefore = DateOnly.FromDateTime(DateTime.UtcNow);

        var filters = PaymentFilters.WithSpecifications()
            .WithCreatedAfter(createdAfter)
            .WithCreatedBefore(createdBefore)
            .Build();

        var pipeline = PipelineDefinitionBuilder
            .For<Payment>()
            .As<Payment, Payment, BsonDocument>()
            .FilterPayments(filters);

        var rendered = pipeline.Render(new(
            BsonSerializer.SerializerRegistry.GetSerializer<Payment>(),
            BsonSerializer.SerializerRegistry));

        Assert.Single(rendered.Documents);

        var match = rendered.Documents[0]["$match"].AsBsonDocument;
        var createdAt = match[Documents.Payment.CreatedAt].AsBsonDocument;

        Assert.True(createdAt.Contains("$gte"));
        Assert.True(createdAt.Contains("$lte"));

        Assert.Equal(createdAfter, DateOnly.FromDateTime(createdAt["$gte"].ToUniversalTime()));
        Assert.Equal(createdBefore, DateOnly.FromDateTime(createdAt["$lte"].ToUniversalTime()));
    }

    [Fact(DisplayName = "[filters] - when amount range is provided then it should be included in the filter definition")]
    public void When_AmountRangeIsProvided_ThenItShouldBeIncludedInTheFilterDefinition()
    {
        const decimal minAmount = 10.5m;
        const decimal maxAmount = 72.9m;

        var filters = PaymentFilters.WithSpecifications()
            .WithMinAmount(minAmount)
            .WithMaxAmount(maxAmount)
            .Build();

        var pipeline = PipelineDefinitionBuilder
            .For<Payment>()
            .As<Payment, Payment, BsonDocument>()
            .FilterPayments(filters);

        var rendered = pipeline.Render(new(
            BsonSerializer.SerializerRegistry.GetSerializer<Payment>(),
            BsonSerializer.SerializerRegistry));

        Assert.Single(rendered.Documents);

        var match = rendered.Documents[0]["$match"].AsBsonDocument;
        var amount = match[Documents.Payment.Amount].AsBsonDocument;

        Assert.True(amount.Contains("$gte"));
        Assert.True(amount.Contains("$lte"));

        Assert.Equal(minAmount, amount["$gte"].ToDecimal());
        Assert.Equal(maxAmount, amount["$lte"].ToDecimal());
    }

    [Fact(DisplayName = "[filters] - when all specifications are provided then all filters should be included in the filter definition")]
    public void When_AllSpecificationsAreProvided_ThenAllFiltersShouldBeIncludedInTheFilterDefinition()
    {
        var identifier = Identifier.Generate<Payment>();
        var payerId = Identifier.Generate<User>();

        var externalId = Identifier.Generate<Payment>();
        var referenceId = Identifier.Generate<Payment>();

        var status = Status.Refunded;
        var method = Method.Pix;

        var createdAfter = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        var createdBefore = DateOnly.FromDateTime(DateTime.UtcNow);

        const decimal minAmount = 19.9m;
        const decimal maxAmount = 149.9m;

        var filters = PaymentFilters.WithSpecifications()
            .WithIdentifier(identifier)
            .WithPayerId(payerId)
            .WithExternalId(externalId)
            .WithReferenceId(referenceId)
            .WithStatus(status)
            .WithMethod(method)
            .WithCreatedAfter(createdAfter)
            .WithCreatedBefore(createdBefore)
            .WithMinAmount(minAmount)
            .WithMaxAmount(maxAmount)
            .Build();

        var pipeline = PipelineDefinitionBuilder
            .For<Payment>()
            .As<Payment, Payment, BsonDocument>()
            .FilterPayments(filters);

        var rendered = pipeline.Render(new(
            BsonSerializer.SerializerRegistry.GetSerializer<Payment>(),
            BsonSerializer.SerializerRegistry));

        Assert.Single(rendered.Documents);

        var match = rendered.Documents[0]["$match"].AsBsonDocument;

        Assert.Equal(identifier, match[Documents.Payment.Identifier].AsString);
        Assert.Equal(payerId, match[Documents.Payment.PayerId].AsString);

        Assert.Equal(externalId, match[Documents.Payment.ExternalId].AsString);
        Assert.Equal(referenceId, match[Documents.Payment.ReferenceId].AsString);

        Assert.Equal((int)status, match[Documents.Payment.Status].ToInt32());
        Assert.Equal((int)method, match[Documents.Payment.Method].ToInt32());

        var createdAt = match[Documents.Payment.CreatedAt].AsBsonDocument;
        var amount = match[Documents.Payment.Amount].AsBsonDocument;

        Assert.Equal(createdAfter, DateOnly.FromDateTime(createdAt["$gte"].ToUniversalTime()));
        Assert.Equal(createdBefore, DateOnly.FromDateTime(createdAt["$lte"].ToUniversalTime()));

        Assert.Equal(minAmount, amount["$gte"].ToDecimal());
        Assert.Equal(maxAmount, amount["$lte"].ToDecimal());
    }
}
