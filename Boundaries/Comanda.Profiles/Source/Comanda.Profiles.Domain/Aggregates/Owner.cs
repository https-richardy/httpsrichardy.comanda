namespace Comanda.Profiles.Domain.Aggregates;

public sealed class Owner : Aggregate
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public User User { get; set; } = default!;
    public Contact Contact { get; set; } = default!;
}
