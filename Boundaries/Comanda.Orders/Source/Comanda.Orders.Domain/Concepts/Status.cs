namespace Comanda.Orders.Domain.Concepts;

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
