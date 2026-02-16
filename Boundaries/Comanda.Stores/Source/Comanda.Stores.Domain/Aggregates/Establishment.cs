namespace Comanda.Stores.Domain.Aggregates;

public sealed class Establishment : Aggregate
{
    public Properties Properties { get; set; } = default!;
    public Branding Branding { get; set; } = default!;
    public User Owner { get; set; } = default!;
    public ICollection<Credential> Credentials { get; set; } = [];

    public void WithChanges(Action<EstablishmentBuilder> action) =>
        action(new EstablishmentBuilder(this));
}
