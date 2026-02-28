namespace Comanda.Merchants.WebUI.Forms.Schemes;

public sealed record EstablishmentUpdateFormScheme
{
    [Required(ErrorMessage = "O nome do estabelecimento é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 500 caracteres")]
    public string Description { get; set; } = string.Empty;

    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3}|[A-Fa-f0-9]{8})$", ErrorMessage = "Cor primária deve ser um código hexadecimal válido (ex: #4a1998)")]
    public string PrimaryColor { get; set; } = "#5B21B6";

    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3}|[A-Fa-f0-9]{8})$", ErrorMessage = "Cor secundária deve ser um código hexadecimal válido (ex: #ec4899)")]
    public string SecondaryColor { get; set; } = "#EC4899";
    public string Logo { get; set; } = string.Empty;
}
