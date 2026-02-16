namespace Comanda.Subscriptions.Domain.Filtering.Builders;

public sealed class SubscriptionFiltersBuilder :
    FiltersBuilderBase<SubscriptionFilters, SubscriptionFiltersBuilder>
{
    public SubscriptionFiltersBuilder WithSubscriberId(string? subscriberId)
    {
        _filters.SubscriberId = subscriberId;
        return this;
    }

    public SubscriptionFiltersBuilder WithExternalId(string? externalId)
    {
        _filters.ExternalId = externalId;
        return this;
    }

    public SubscriptionFiltersBuilder WithPlan(Plan? plan)
    {
        _filters.Plan = plan;
        return this;
    }

    public SubscriptionFiltersBuilder WithStatus(Status? status)
    {
        _filters.Status = status;
        return this;
    }
}
