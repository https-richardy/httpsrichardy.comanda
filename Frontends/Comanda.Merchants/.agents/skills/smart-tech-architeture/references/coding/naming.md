# Convenções de Nomenclatura

## Arquivos e Pastas

| Tipo           | Padrão      | Exemplo                      |
|----------------|-------------|------------------------------|
| Arquivos       | kebab-case  | `use-operators.ts`           |
| Pastas         | kebab-case  | `features/operator-groups/`  |
| Componentes    | kebab-case  | `operator-list.tsx`          |

## Exports de Componentes

Componentes sempre exportados como `PascalCase`:
```ts
// arquivo: operator-list.tsx
export function OperatorList() { ... }
```

## Sufixos Obrigatórios

| Sufixo        | Quando usar                          | Exemplo                  |
|---------------|--------------------------------------|--------------------------|
| `*.dto.ts`    | Tipagem do contrato da API           | `operator.dto.ts`        |
| `*.model.ts`  | Tipagem do modelo da UI              | `operator.model.ts`      |
| `*.mapper.ts` | Funções de conversão DTO ↔ Model     | `operator.mapper.ts`     |
| `*.test.ts`   | Arquivos de teste                    | `operator.test.ts`       |
| `use-*.ts`    | Hooks React                          | `use-operators.ts`       |

## Hooks

- Sempre prefixo `use-`
- Nome descreve o dado ou ação: `use-operators`, `use-create-operator`
- Mutations usam verbo: `use-create-*`, `use-update-*`, `use-delete-*`

## Query Keys (TanStack Query)

- Array de strings em `kebab-case`
- Começa pelo nome da feature: `['operators']`, `['operators', id]`
- Hierárquico para invalidação granular: `['operators', 'detail', id]`