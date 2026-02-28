namespace Comanda.Merchants.WebUI.Forms.Schemes;

public sealed record ProductCreationFormScheme
{
    [Required(ErrorMessage = "O título do produto é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 100 caracteres")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 500 caracteres")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "O preço é obrigatório")]
    [Range(0.01, 999999.99, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal Price { get; set; }
}
