namespace Comanda.Stores.Application.Validators.Product;

public sealed class ProductImageUploadSchemeValidator : AbstractValidator<ProductImageUploadScheme>
{
    public ProductImageUploadSchemeValidator()
    {
        RuleFor(file => file.Stream)
            .NotNull()
            .WithMessage("file stream must not be null.")
            .Must(stream => stream.CanRead)
            .WithMessage("file stream is not readable.")
            .Must(stream => stream.Length > 0)
            .WithMessage("file stream is empty.")
            .Must(stream => stream.Length <= 20 * 1024 * 1024)
            .WithMessage("file must be at most 20MB.");
    }
}
