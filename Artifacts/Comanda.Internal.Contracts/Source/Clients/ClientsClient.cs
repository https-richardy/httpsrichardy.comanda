namespace Comanda.Internal.Contracts.Clients;

public sealed class ClientsClient(HttpClient httpClient) : IClientsClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly JsonSerializerOptions _serializer = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
    };

    public async Task<Result<Pagination<ClientScheme>>> GetClientsAsync(ClientFiltersParameters parameters, CancellationToken cancellation = default)
    {
        var offset = (parameters.Page - 1) * parameters.PageSize;
        var search = string.IsNullOrWhiteSpace(parameters.ClientId)
            ? string.Empty
            : $"&clientId={Uri.EscapeDataString(parameters.ClientId)}";

        var response = await _httpClient.GetAsync($"clients?first={offset}&max={parameters.PageSize}{search}", cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            return Result<Pagination<ClientScheme>>.Failure(Error.Unknown);
        }

        var clients = JsonSerializer.Deserialize<IReadOnlyCollection<ClientScheme>>(content, _serializer);
        if (clients is null)
        {
            return Result<Pagination<ClientScheme>>.Failure(Error.Unknown);
        }

        var pagination = new Pagination<ClientScheme>()
            .WithItems(clients)
            .WithPageNumber(parameters.Page)
            .WithPageSize(parameters.PageSize)
            .WithTotal(clients.Count);

        return Result<Pagination<ClientScheme>>.Success(pagination);
    }

    public async Task<Result<ClientScheme>> GetClientAsync(string id, CancellationToken cancellation = default)
    {
        var response = await _httpClient.GetAsync($"clients/{id}", cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            return Result<ClientScheme>.Failure(AuthorizationErrors.ClientDoesNotExist);
        }

        var client = JsonSerializer.Deserialize<ClientScheme>(content, _serializer);

        return client is null
            ? Result<ClientScheme>.Failure(AuthorizationErrors.ClientDoesNotExist)
            : Result<ClientScheme>.Success(client);
    }
}
