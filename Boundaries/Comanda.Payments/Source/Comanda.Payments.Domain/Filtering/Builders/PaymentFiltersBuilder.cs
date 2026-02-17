namespace Comanda.Payments.Domain.Filtering.Builders;

public sealed class PaymentFiltersBuilder :
    FiltersBuilderBase<PaymentFilters, PaymentFiltersBuilder>
{
    public PaymentFiltersBuilder WithPayerId(string? payerId)
    {
        _filters.PayerId = payerId;
        return this;
    }

    public PaymentFiltersBuilder WithExternalId(string? externalId)
    {
        _filters.ExternalId = externalId;
        return this;
    }

    public PaymentFiltersBuilder WithReferenceId(string? referenceId)
    {
        _filters.ReferenceId = referenceId;
        return this;
    }

    public PaymentFiltersBuilder WithStatus(Status? status)
    {
        _filters.Status = status;
        return this;
    }

    public PaymentFiltersBuilder WithMethod(Method? method)
    {
        _filters.Method = method;
        return this;
    }

    public PaymentFiltersBuilder WithMinAmount(decimal? minAmount)
    {
        _filters.MinAmount = minAmount;
        return this;
    }

    public PaymentFiltersBuilder WithMaxAmount(decimal? maxAmount)
    {
        _filters.MaxAmount = maxAmount;
        return this;
    }
}
