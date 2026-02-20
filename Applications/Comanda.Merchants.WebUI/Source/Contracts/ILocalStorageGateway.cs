namespace Comanda.Merchants.WebUI.Contracts;

public interface ILocalStorageGateway
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellation = default);
    public Task<string?> GetAsStringAsync(string key, CancellationToken cancellation = default);

    public Task SetAsync<T>(string key, T value, CancellationToken cancellation = default);
    public Task SetAsStringAsync(string key, string value, CancellationToken cancellation = default);

    public Task RemoveAsync(string key, CancellationToken cancellation = default);
    public Task ClearAsync(CancellationToken cancellation = default);
}
