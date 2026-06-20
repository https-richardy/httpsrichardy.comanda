# Regras de Imports e Dependências

## Alias de Importação

Sempre usar `@/` para imports absolutos a partir de `src/`:
```ts
import { httpClient } from '@/infra/api'
import { Button } from '@/shared/components/ui/button'
import { useOperators } from '../hooks/use-operators' // relativo dentro da feature
```

## Limites de Dependência

### Features
- ✅ Podem importar de `shared/` e `infra/`
- ✅ Podem usar imports relativos dentro da própria feature
- ❌ **Nunca** importar de outra feature

```ts
// ✅ Correto
import { Datatable } from '@/shared/components/common/datatable'

// ❌ Errado — feature importando de outra feature
import { usePartners } from '@/features/partners/hooks/use-partners'
```

### Shared
- ✅ Pode importar de `infra/`
- ❌ Não importa de `features/`

### Infra
- ✅ Independente — não importa de nenhuma outra camada do projeto

## Ordem de Imports (padrão do projeto)

1. Bibliotecas externas (`react`, `@tanstack/react-query`, etc.)
2. Imports absolutos internos (`@/infra/...`, `@/shared/...`)
3. Imports relativos da própria feature (`../hooks/...`, `./mapper`)

```ts
import { useQuery } from '@tanstack/react-query'

import { httpClient } from '@/infra/api'
import { Button } from '@/shared/components/ui/button'

import { toModel } from './mapper'
import type { Operator } from '../types/operator.model'
```