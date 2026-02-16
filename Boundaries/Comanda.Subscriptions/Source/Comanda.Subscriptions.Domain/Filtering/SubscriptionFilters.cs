namespace Comanda.Subscriptions.Domain.Filtering;

public sealed class SubscriptionFilters : Filters
{
    public string? SubscriberId { get; set; }
    public string? ExternalId { get; set; }

    public Plan? Plan { get; set; }
    public Status? Status { get; set; }

    public static SubscriptionFiltersBuilder WithSpecifications() => new();
}
