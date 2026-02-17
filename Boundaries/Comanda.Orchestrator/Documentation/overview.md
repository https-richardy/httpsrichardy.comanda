# Comanda.Orchestrator - Overview

O projeto **Comanda.Orchestrator** atua como um BFF (Backend For Frontend) e também incorpora características de um API Gateway. Sua principal responsabilidade é orquestrar chamadas entre diferentes serviços e APIs do sistema **Comanda**, centralizando o acesso e simplificando a comunicação entre o frontend e os diversos backends.

## Principais Responsabilidades
- **Orquestração de Chamadas:** Centraliza e coordena as requisições entre múltiplos serviços, reunindo dados e respostas para o frontend de forma eficiente.

- **API Gateway:** Expõe endpoints unificados para o frontend, abstraindo a complexidade dos serviços internos e facilitando a integração.

- **Aplicação de Patterns de Resiliência:** Implementa padrões de resiliência, como circuit breaker, retry, timeout e fallback, garantindo maior robustez e disponibilidade. Esses padrões podem ser observados, por exemplo, nos gateways implementados na camada de infraestrutura.
