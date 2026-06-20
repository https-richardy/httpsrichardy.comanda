# Lint e Formatação

## Ferramentas

- **ESLint** — análise estática
- **Prettier** — formatação de código

## Scripts

```bash
pnpm lint          # verifica erros de lint
pnpm lint --fix    # corrige automaticamente o que for possível
```

## Convenções de Código

### TypeScript
- Sempre tipar props de componentes com `interface`
- Preferir `type` para unions e interseções, `interface` para objetos
- Evitar `any` — usar `unknown` quando o tipo não for conhecido

```ts
// ✅
interface OperatorProps {
  id: string
  onSelect: (id: string) => void
}

// ❌
const handleSelect = (data: any) => { ... }
```

### Imports
- Sem imports não utilizados
- Imports de tipos devem usar `import type`:

```ts
import type { Operator } from '../types/operator.model'
```

### Componentes
- Sempre usar arrow functions para componentes internos e callbacks
- Componentes exportados como function declaration:

```tsx
// ✅ Export
export function OperatorList() { ... }

// ✅ Callback interno
const handleClick = () => { ... }
```

## Formatação (Prettier)

Configuração padrão do projeto:
- Aspas simples (`'`)
- Ponto e vírgula: não
- Largura de linha: 100
- Trailing comma: `es5`