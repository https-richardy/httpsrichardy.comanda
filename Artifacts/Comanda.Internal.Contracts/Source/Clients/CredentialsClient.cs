namespace Comanda.Internal.Contracts.Clients;

public sealed class CredentialsClient(HttpClient httpClient) : ICredentialsClient
{
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<Result<IReadOnlyCollection<CredentialScheme>>> GetCredentialsAsync(
        CredentialsFetchParameters parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.GetAsync($"establishments/{parameters.EstablishmentId}/integrations/credentials", cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.NotFound => EstablishmentErrors.EstablishmentDoesNotExist,
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<IReadOnlyCollection<CredentialScheme>>.Failure(error);
        }

        var credentials = JsonSerializer.Deserialize<IReadOnlyCollection<CredentialScheme>>(content, serializerOptions);
        if (credentials is null)
        {
            return Result<IReadOnlyCollection<CredentialScheme>>.Failure(CommonErrors.InvalidContent);
        }

        return Result<IReadOnlyCollection<CredentialScheme>>.Success(credentials);
    }

    public async Task<Result<CredentialScheme>> AssignIntegrationCredentialAsync(
        CredentialCreationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync($"establishments/{parameters.EstablishmentId}/integrations/credentials", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.NotFound => EstablishmentErrors.EstablishmentDoesNotExist,
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<CredentialScheme>.Failure(error);
        }

        var credential = JsonSerializer.Deserialize<CredentialScheme>(content, serializerOptions);
        if (credential is null)
        {
            return Result<CredentialScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<CredentialScheme>.Success(credential);
    }

    public async Task<Result<CredentialScheme>> UpdateCredentialAsync(
        CredentialModificationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PutAsJsonAsync($"establishments/{parameters.EstablishmentId}/integrations/credentials/{parameters.CredentialId}", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonSerializer.Deserialize<Error>(content, serializerOptions);
            if (error is not null)
                return Result<CredentialScheme>.Failure(error);

            return Result<CredentialScheme>.Failure(CommonErrors.OperationFailed);
        }

        var credential = JsonSerializer.Deserialize<CredentialScheme>(content, serializerOptions);
        if (credential is null)
        {
            return Result<CredentialScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<CredentialScheme>.Success(credential);
    }
}
