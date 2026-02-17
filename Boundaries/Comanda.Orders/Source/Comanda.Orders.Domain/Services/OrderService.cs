namespace Comanda.Orders.Domain.Services;

public sealed class OrderService : IOrderService
{
    public async Task<Code> GenerateCodeAsync(CancellationToken cancellation = default)
    {
        return await Task.FromResult(Identifier.FromMask("DDDDDADADA", TimeSpan.Zero));
    }
}
