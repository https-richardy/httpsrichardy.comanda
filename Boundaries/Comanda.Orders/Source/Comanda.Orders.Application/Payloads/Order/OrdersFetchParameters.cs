namespace Comanda.Orders.Application.Payloads.Order;

public sealed record OrdersFetchParameters :
    IDispatchable<Result<PaginationScheme<OrderScheme>>>
{
    public string? Id { get; set; }
    public string? Code { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public string? Fulfillment { get; set; }

    public string? MerchantId { get; set; }
    public string? ConsumerId { get; set; }

    public PaginationFilters? Pagination { get; set; }
    public SortFilters? Sort { get; set; }

    public DateOnly? CreatedAfter { get; set; }
    public DateOnly? CreatedBefore { get; set; }
}
