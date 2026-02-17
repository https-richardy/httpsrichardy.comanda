namespace Comanda.Orchestrator.WebApi.Extensions;

public static class StreamExtension
{
    public static ProductImageStreamScheme AsImage(this Stream stream, string productId, string establishmentId)
    {
        return new ProductImageStreamScheme
        {
            ProductId = productId,
            EstablishmentId = establishmentId,
            Stream = stream
        };
    }
}
