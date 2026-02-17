namespace Comanda.Payments.Domain.Filtering;

public sealed class PaymentFilters : Filters
{
    public string? PayerId { get; set; }
    public string? ExternalId { get; set; }
    public string? ReferenceId { get; set; }

    public Status? Status { get; set; }
    public Method? Method { get; set; }

    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }

    public static PaymentFiltersBuilder WithSpecifications() => new();
}
