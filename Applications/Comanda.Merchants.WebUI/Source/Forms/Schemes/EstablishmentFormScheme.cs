namespace Comanda.Merchants.WebUI.Forms.Schemes;

public sealed record EstablishmentFormScheme
{
    [Required(ErrorMessage = "O nome do estabelecimento é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 500 caracteres")]
    public string Description { get; set; } = string.Empty;
}
