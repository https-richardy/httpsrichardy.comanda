namespace Comanda.Orchestrator.Application.Validators.Products;

public sealed class ProductModificationSchemeValidator : AbstractValidator<ProductModificationScheme>
{
    public ProductModificationSchemeValidator()
    {
        RuleFor(product => product.Title)
            .NotEmpty()
            .WithMessage("product title must be provided.")
            .MaximumLength(100)
            .WithMessage("product title must not exceed 100 characters.");

        RuleFor(product => product.Description)
            .NotEmpty()
            .WithMessage("product description must be provided.")
            .MaximumLength(500)
            .WithMessage("product description must not exceed 500 characters.");

        RuleFor(product => product.Price)
            .GreaterThan(0)
            .WithMessage("product price must be greater than 0.");
    }
}
