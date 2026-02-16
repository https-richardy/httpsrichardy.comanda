namespace Comanda.Subscriptions.Domain.Collections;

public interface ISubscriptionCollection : IAggregateCollection<Subscription>
{
    public Task<IReadOnlyCollection<Subscription>> FilterSubscriptionsAsync(
        SubscriptionFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountSubscriptionsAsync(
        SubscriptionFilters filters,
        CancellationToken cancellation = default
    );
}
