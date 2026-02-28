namespace Comanda.Merchants.WebUI.Forms.Schemes;

public sealed record IntegrationCredentialsFormScheme
{
    [Required(ErrorMessage = "A chave secreta do gateway de pagamentos é obrigatória")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "A chave secreta deve ter entre 10 e 500 caracteres")]
    public string PaymentGatewaySecretKey { get; set; } = string.Empty;
    public string WhatsappToken { get; set; } = string.Empty;
}
