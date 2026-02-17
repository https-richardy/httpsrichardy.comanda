namespace Comanda.Orchestrator.Application.Validators.Orders;

public sealed class OrderCreationSchemeValidator : AbstractValidator<OrderCreationScheme>
{
    public OrderCreationSchemeValidator()
    {
        RuleFor(order => order.Items)
            .NotEmpty()
            .WithMessage("order must have at least one item.");

        RuleFor(order => order.Fulfillment)
            .IsInEnum()
            .WithMessage("fulfillment must be a valid enum value.");

        RuleFor(order => order.Priority)
            .IsInEnum()
            .WithMessage("priority must be a valid enum value.");

        RuleFor(order => order.Metadata)
            .NotNull()
            .WithMessage("metadata must be provided.");

        When(order => order.Metadata is not null, () =>
        {
            RuleFor(order => order.Metadata!.MerchantId)
                .NotEmpty()
                .WithMessage("merchantId must be provided.");

            RuleFor(order => order.Metadata!.ConsumerId)
                .NotEmpty()
                .WithMessage("consumerId must be provided.");
        });

        When(order => order.Items is not null && order.Items.Any(), () =>
        {
            RuleForEach(order => order.Items).ChildRules(item =>
            {
                item.RuleFor(item => item.Title)
                    .NotEmpty()
                    .WithMessage("item title must be provided.");

                item.RuleFor(item => item.Quantity)
                    .GreaterThan(0)
                    .WithMessage("item quantity must be greater than zero.");

                item.RuleFor(item => item.UnitPrice)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("item unitPrice must be zero or greater.");
            });
        });
    }
}
