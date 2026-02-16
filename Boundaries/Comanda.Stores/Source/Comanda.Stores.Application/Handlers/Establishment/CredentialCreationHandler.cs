namespace Comanda.Stores.Application.Handlers.Establishment;

public sealed class CredentialCreationHandler(ICredentialCollection credentialCollection, IEstablishmentCollection establishmentCollection) :
    IDispatchHandler<CredentialCreationScheme, Result<CredentialScheme>>
{
    public async Task<Result<CredentialScheme>> HandleAsync(
        CredentialCreationScheme parameters, CancellationToken cancellation = default)
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

        var credential = await credentialCollection.InsertAsync(parameters.AsCredential(), cancellation: cancellation);

        establishment.WithChanges(builder =>
        {
            builder.SetCredential(credential);
        });

        await establishmentCollection.UpdateAsync(establishment, cancellation);

        return Result<CredentialScheme>.Success(credential.AsResponse());
    }
}
