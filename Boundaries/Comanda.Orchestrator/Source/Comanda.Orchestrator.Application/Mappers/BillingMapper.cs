using Status = Comanda.Internal.Contracts.Transport.Internal.Orders.Status;

namespace Comanda.Orchestrator.Application.Mappers;

public static class BillingMapper
{
    public static PaymentsFetchParameters AsFilters(this string identifier) => new()
    {
        Id = identifier
    };

    public static PaymentStatusUpdateScheme AsMutation(this PaymentScheme payment) => new()
    {
        Identifier = payment.Identifier,
        Status = Internal.Contracts.Transport.Internal.Payments.Status.Paid
    };

    public static OrdersFetchParameters AsFilters(this PaymentScheme payment) => new()
    {
        Id = payment.Reference
    };

    public static OrderModificationScheme AsPatch(this OrderScheme order) => new()
    {
        Id = order.Identifier,
        Status = Status.Confirmed
    };
}
