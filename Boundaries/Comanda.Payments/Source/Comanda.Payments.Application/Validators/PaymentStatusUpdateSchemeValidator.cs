namespace Comanda.Payments.Application.Validators;

public sealed class PaymentStatusUpdateSchemeValidator : AbstractValidator<PaymentStatusUpdateScheme>
{
    public PaymentStatusUpdateSchemeValidator()
    {
        RuleFor(request => request.Status)
            .IsInEnum()
            .WithMessage("payment status must be a valid status value.");
    }
}
