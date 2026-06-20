# Padrões de Teste

## Ferramentas

- **Vitest** — runner e assertions
- **React Testing Library** — testes de componentes

## O Que Testar

| Camada       | O que testar                                      | Prioridade |
|--------------|---------------------------------------------------|------------|
| Mappers      | Conversão correta de DTO → Model e Model → DTO    | Alta       |
| Utils        | Funções puras e transformações                    | Alta       |
| Hooks        | Integração com React Query (com mock do service)  | Média      |
| Components   | Interações do usuário e renderização condicional  | Média      |
| Services     | Apenas se houver lógica além da chamada HTTP      | Baixa      |

## Testando Mappers

```ts
// services/mapper.test.ts
import { describe, it, expect } from 'vitest'
import { toModel, toDTO } from './mapper'
import type { OperatorDTO } from '../types/operator.dto'

describe('operator mapper', () => {
  it('converte DTO para Model corretamente', () => {
    const dto: OperatorDTO = {
      id: '1',
      full_name: 'João Silva',
      status: 'active',
    }

    const model = toModel(dto)

    expect(model.id).toBe('1')
    expect(model.name).toBe('João Silva')
    expect(model.isActive).toBe(true)
  })

  it('converte Model para DTO corretamente', () => {
    const model = { id: '1', name: 'João Silva', isActive: false }
    const dto = toDTO(model)
    expect(dto.status).toBe('inactive')
  })
})
```

## Testando Componentes

```tsx
// components/operator-list.test.tsx
import { render, screen } from '@testing-library/react'
import { vi } from 'vitest'
import { OperatorList } from './operator-list'
import * as hooks from '../hooks/use-operators'

describe('OperatorList', () => {
  it('exibe loading enquanto carrega', () => {
    vi.spyOn(hooks, 'useOperators').mockReturnValue({
      data: undefined,
      isLoading: true,
    } as any)

    render(<OperatorList />)
    expect(screen.getByText('Carregando...')).toBeInTheDocument()
  })

  it('exibe lista de operadores', () => {
    vi.spyOn(hooks, 'useOperators').mockReturnValue({
      data: [{ id: '1', name: 'João' }],
      isLoading: false,
    } as any)

    render(<OperatorList />)
    expect(screen.getByText('João')).toBeInTheDocument()
  })
})
```

## Convenções

- Arquivo de teste junto ao arquivo testado: `operator-list.test.tsx`
- Descrever comportamentos, não implementação: `'exibe erro quando API falha'`
- Mockar sempre os hooks nos testes de componente — não o `httpClient`