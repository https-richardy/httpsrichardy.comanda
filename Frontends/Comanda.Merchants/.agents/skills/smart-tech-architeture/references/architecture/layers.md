# Camadas e Responsabilidades

## Visão Geral

```
React Components
      ↓
Feature Hooks
      ↓
Feature Services
      ↓
Infra Layer (API / SSE / WebSocket)
      ↓
Backend
```

## Responsabilidades por Camada

### Components
- Renderiza UI e captura interações do usuário
- Recebe dados exclusivamente via hooks
- Não faz chamadas HTTP, não acessa stores diretamente para dados de servidor

### Hooks
- Orquestra dados usando TanStack Query (queries e mutations)
- Pode acessar Zustand para estado global de UI
- Chama services — nunca o `httpClient` diretamente

### Services (`services/index.ts`)
- Único lugar onde chamadas HTTP acontecem
- Converte DTO → Model via mapper antes de retornar
- Converte Model → DTO via mapper antes de enviar

### Mappers (`services/mapper.ts`)
- Converte entre o formato da API (DTO) e o formato da UI (Model)
- Sem lógica de negócio — apenas transformação de dados

### Infra (`infra/api`, `infra/sse`)
- Configura o cliente HTTP base (axios) com interceptores
- Gerencia autenticação, headers e tratamento global de erros HTTP
- Clientes SSE/SignalR atualizam o cache do React Query diretamente

## Regras de Comunicação entre Camadas

| De          | Pode chamar         | Não pode chamar          |
|-------------|---------------------|--------------------------|
| Component   | Hook da feature     | Service, httpClient      |
| Hook        | Service da feature  | httpClient diretamente   |
| Service     | httpClient (infra)  | Hook, Component          |
| Mapper      | —                   | Qualquer outra camada    |
| Infra       | Backend externo     | Hook, Service, Component, Store |

> **Nota sobre `infra/`:** a camada de infra nunca importa stores ou hooks. Dependências como `getToken` e `onUnauthorized` são **injetadas via parâmetro** pela camada `app/` no bootstrap da aplicação.