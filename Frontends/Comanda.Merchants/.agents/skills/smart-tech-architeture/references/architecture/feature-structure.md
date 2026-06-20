# Organização Interna de uma Feature

Cada feature vive em `features/<nome>/` e é **autocontida** — sem dependências de outras features.

## Estrutura de Pastas

```
features/buy-simulation/
├── components/
│   ├── buy-simulation-form/
│   │   ├── buy-simulation-form.tsx
│   │   └── use-buy-simulation-form.ts  # hook exclusivo do componente
│   └── buy-simulation-content.tsx
├── hooks/
│   ├── use-queries.ts                  # todas as queries da feature
│   ├── use-mutations.ts                # todas as mutations da feature
│   └── use-buy-simulation.ts           # contexto compartilhado entre componentes
├── services/
│   ├── index.ts           # Chamadas HTTP
│   ├── mapper.ts          # Conversão DTO ⇄ Model
│   └── error-handler.ts   # Erros de domínio
└── types/
    ├── buy-simulation.dto.ts
    └── buy-simulation.model.ts
```

## Responsabilidade de Cada Arquivo

| Arquivo                         | Função                                                    |
|---------------------------------|-----------------------------------------------------------|
| `types/*.dto.ts`                | Tipagem do contrato da API                                |
| `types/*.model.ts`              | Tipagem do modelo usado pela UI                           |
| `services/index.ts`             | Funções de chamada HTTP (get, create, update…)            |
| `services/mapper.ts`            | Conversão bidirecional DTO ↔ Model                        |
| `services/error-handler.ts`     | Tradução de erros da API para mensagens de UI             |
| `hooks/use-queries.ts`          | Todas as queries (leitura) com TanStack Query             |
| `hooks/use-mutations.ts`        | Todas as mutations (escrita) com TanStack Query           |
| `hooks/use-<context>.ts`        | Contexto/hook compartilhado por múltiplos componentes     |
| `components/<n>/use-<n>.ts`     | Hook exclusivo de um único componente (ex: formulário)    |
| `components/`                   | UI da feature, consome hooks                              |

## Onde colocar cada hook

A decisão de onde colocar um hook depende de **quantos componentes o consomem**:

| Situação                                              | Onde colocar                          |
|-------------------------------------------------------|---------------------------------------|
| Usado por apenas um componente (ex: lógica de form)   | `components/<n>/use-<n>.ts`           |
| Compartilhado por dois ou mais componentes da feature | `hooks/use-<context>.ts`              |
| Queries TanStack Query                                | `hooks/use-queries.ts`                |
| Mutations TanStack Query                              | `hooks/use-mutations.ts`              |

**Exemplo:** `use-buy-simulation.ts` fica em `hooks/` porque tanto o form quanto o card o consomem:

```
hooks/use-buy-simulation.ts   ← compartilhado: form + card consomem
components/
  buy-simulation-form/
    use-buy-simulation-form.ts ← exclusivo: só o form usa
  buy-simulation-content.tsx   ← consome use-buy-simulation via import
```

## Regras

- Features **não importam** de outras features
- `shared/` e `infra/` podem ser importados livremente
- Toda nova feature segue exatamente essa estrutura — sem omitir camadas
- `hooks/use-queries.ts` e `hooks/use-mutations.ts` são os únicos pontos de contato com TanStack Query na feature
- Hook usado por **um único componente** → fica junto ao componente (`components/<n>/use-<n>.ts`)
- Hook usado por **dois ou mais componentes** → fica em `hooks/use-<context>.ts`