namespace Comanda.Stores.Application.Handlers.Establishment;

public sealed class CredentialEditHandler(ICredentialCollection credentialCollection, IEstablishmentCollection establishmentCollection) :
    IDispatchHandler<CredentialEditScheme, Result<CredentialScheme>>
{
    public async Task<Result<CredentialScheme>> HandleAsync(
        CredentialEditScheme parameters, CancellationToken cancellation = default)
    {
        var filters = EstablishmentFilters.WithSpecifications()
            .WithIdentifier(parameters.EstablishmentId)
            .Build();

        var establishments = await establishmentCollection.FilterEstablishmentsAsync(filters, cancellation);
        var establishment = establishments.FirstOrDefault();

        if (establishment is null)
        {
            return Result<CredentialScheme>.Failure(EstablishmentErrors.EstablishmentDoesNotExist);
        }

        var existingCredential = establishment.Credentials
            .FirstOrDefault(credential => credential.Id == parameters.CredentialId);

        if (existingCredential is null)
        {
            return Result<CredentialScheme>.Failure(CredentialErrors.CredentialDoesNotExist);
        }

        existingCredential.WithChanges(builder =>
        {
            builder.SetProvider(parameters.Provider);
            builder.SetSecretKey(parameters.SecretKey);
        });

        var credential = await credentialCollection.UpdateAsync(existingCredential, cancellation);

        return Result<CredentialScheme>.Success(credential.AsResponse());
    }
}
