namespace Comanda.Internal.Contracts.Transport.Internal.Orders;

public enum Status
{
    Pending,
    Confirmed,
    InPreparation,
    Ready,
    Finalized,
    Cancelled,
    Failed,
    Refunded,
    Returned
}
