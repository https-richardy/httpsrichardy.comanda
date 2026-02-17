namespace Comanda.Orders.Domain.Contracts;

public interface IOrderService
{
    public Task<Code> GenerateCodeAsync(
        CancellationToken cancellation = default
    );
}
