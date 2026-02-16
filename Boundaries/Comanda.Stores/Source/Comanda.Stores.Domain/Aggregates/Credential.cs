namespace Comanda.Stores.Domain.Aggregates;

public sealed class Credential : Aggregate
{
    public IntegrationTarget Provider { get; set; } = default!;
    public string SecretKey { get; set; } = default!;

    public void WithChanges(Action<CredentialBuilder> action) =>
        action(new CredentialBuilder(this));
}
