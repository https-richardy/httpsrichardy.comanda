namespace Comanda.Orders.Application.Validators;

public sealed class OrderModificationSchemeValidator : AbstractValidator<OrderModificationScheme>
{
    public OrderModificationSchemeValidator()
    {
        RuleFor(order => order.Status)
            .IsInEnum()
            .WithMessage("status must be a valid enum value.");

        RuleFor(order => order.Priority)
            .IsInEnum()
            .WithMessage("priority must be a valid enum value.");
    }
}
