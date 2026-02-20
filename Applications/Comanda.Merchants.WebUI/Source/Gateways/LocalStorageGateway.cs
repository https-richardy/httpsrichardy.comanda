namespace Comanda.Merchants.WebUI.Gateways;

public sealed class LocalStorageGateway(IJSRuntime javascript) : ILocalStorageGateway
{
    public async Task<string?> GetAsStringAsync(string key, CancellationToken cancellation = default)
    {
        return await javascript.InvokeAsync<string?>("localStorage.getItem", key);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellation = default)
    {
        var value = await javascript.InvokeAsync<string?>("localStorage.getItem", key);
        if (string.IsNullOrEmpty(value))
            return default;

        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsStringAsync(string key, string value, CancellationToken cancellation = default)
    {
        await javascript.InvokeVoidAsync("localStorage.setItem", key, value);
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellation = default)
    {
        await javascript.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
    }

    public async Task RemoveAsync(string key, CancellationToken cancellation = default)
    {
        await javascript.InvokeVoidAsync("localStorage.removeItem", key);
    }

    public async Task ClearAsync(CancellationToken cancellation = default)
    {
        await javascript.InvokeVoidAsync("localStorage.clear");
    }
}
