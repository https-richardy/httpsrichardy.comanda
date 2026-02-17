# Padrões de Código - Comanda.Orchestrator

Este documento define os principais padrões e convenções que devem ser seguidos na codebase do Comanda.Orchestrator. O objetivo é garantir consistência, legibilidade e robustez em todo o projeto.

---

## 1. Pattern Matching com Switch nos Controllers

Sempre utilize pattern matching com `switch` para tratar os retornos dos handlers nos controllers. Isso garante padronização no tratamento de respostas e facilita a manutenção.

**Exemplo:**
```csharp
return result switch
{
    { IsSuccess: true } when result.Data is not null =>
        StatusCode(StatusCodes.Status200OK, result.Data),

    { IsFailure: true } when result.Error == AbacatePayErrors.OperationFailed =>
        StatusCode(StatusCodes.Status502BadGateway, result.Error),

    { IsFailure: true } when result.Error == CommonErrors.OperationFailed =>
        StatusCode(StatusCodes.Status500InternalServerError, result.Error),

    { IsFailure: true } when result.Error == CommonErrors.RateLimitExceeded =>
        StatusCode(StatusCodes.Status429TooManyRequests, result.Error)
};
```
> **Importante:** Não utilize `if/else` ou `try/catch` para tratar retornos de handlers nos controllers. Sempre siga o padrão acima.

---

## 2. Encapsulamento de Clients via Gateways

Todo client HTTP/externo deve ser encapsulado por um Gateway.

- **Gateway:** Classe responsável por isolar a comunicação com sistemas externos, aplicar patterns de resiliência e expor métodos claros para uso interno.
- Nunca consuma APIs externas diretamente em handlers ou services.
---

## 3. Patterns de Resiliência com Polly

Toda chamada externa (via Gateway) deve obrigatoriamente aplicar patterns de resiliência usando Polly:

- Retry
- Circuit Breaker
- Timeout
- Fallback

---

## 4. Early Return / Fail Fast

Prefira sempre o uso de early return/fail fast para simplificar o fluxo dos métodos e evitar aninhamentos desnecessários. Isso torna o código mais limpo e fácil de entender.

**Exemplo:**
```csharp
if (!request.IsValid)
    return BadRequest("Dados inválidos");

// lógica principal aqui
```

---

> **Siga estes padrões para garantir qualidade, previsibilidade e robustez em toda a codebase do Comanda.Orchestrator.**
