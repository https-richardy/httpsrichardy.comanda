namespace Comanda.Merchants.WebUI.Contracts;

public interface IPrincipalProvider
{
    public Task<Result<PrincipalScheme>> GetPrincipalAsync(CancellationToken cancellation = default);
}
