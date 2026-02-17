namespace Comanda.Payments.Domain.Concepts;

public sealed record PaymentMetadata(string Identifier, string Reference) :
    IValueObject<PaymentMetadata>
{
    public static PaymentMetadata None =>
        new(string.Empty, string.Empty);
}
