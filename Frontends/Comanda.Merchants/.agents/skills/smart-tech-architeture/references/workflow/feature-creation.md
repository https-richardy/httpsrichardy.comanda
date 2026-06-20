# Passo a Passo: Criar uma Nova Feature

Siga esta ordem ao criar qualquer nova funcionalidade.

## 1. Criar a pasta da feature

```
features/<nome-da-feature>/
```

Use `kebab-case` para o nome.

## 2. Definir os tipos

**`types/<n>.dto.ts`** — contrato da API:
```ts
export interface OperatorDTO {
  id: string
  name: string
  // campos exatamente como a API retorna
}
```

**`types/<n>.model.ts`** — modelo da UI:
```ts
export interface Operator {
  id: string
  name: string
  // campos adaptados para a UI
}
```

## 3. Criar o mapper

**`services/mapper.ts`**:
```ts
import type { OperatorDTO } from '../types/operator.dto'
import type { Operator } from '../types/operator.model'

export class OperatorMapper {
  public static toModel(dto: OperatorDTO): Operator {
    return { id: dto.id, name: dto.name }
  }

  public static toDTO(model: Operator): OperatorDTO {
    return { id: model.id, name: model.name }
  }

  public static toModelList(dtos: OperatorDTO[]): Operator[] {
    return dtos.map(OperatorMapper.toModel)
  }
}
```

## 4. Criar o error handler

**`services/error-handler.ts`**:
```ts
import { AxiosError } from 'axios'

export function handleOperatorError(error: Error | unknown, action: string): never {
  console.error(`Erro em Operator Service [${action}]:`, error)

  let message = 'Ocorreu um erro inesperado na operação.'

  if (error instanceof AxiosError) {
    const status = error.response?.status
    const apiMessage = error.response?.data?.message

    switch (status) {
      case 400:
        message = apiMessage || 'Dados inválidos para esta operação.'
        break
      case 403:
        message = 'Você não tem permissão para realizar esta ação.'
        break
      case 404:
        message = apiMessage || 'Recurso não encontrado.'
        break
      case 409:
        message = apiMessage || 'Conflito: o registro já existe.'
        break
      case 500:
        message = 'Erro interno no servidor. Tente novamente mais tarde.'
        break
    }
  } else if (error instanceof Error) {
    message = error.message
  }

  throw new Error(message)
}
```

O error handler **nunca retorna** — sempre lança (`never`). O erro relançado sobe para o `MutationCache` global, que exibe o toast com `error.message`.

## 5. Criar o service

**`services/index.ts`**:
```ts
import { httpClient } from '@/infra/api'
import { OperatorMapper } from './mapper'
import { handleOperatorError } from './error-handler'
import type { Operator } from '../types/operator.model'

export class OperatorService {
  public static async getOperators(): Promise<Operator[]> {
    try {
      const { data } = await httpClient.get('/operators')
      return OperatorMapper.toModelList(data)
    } catch (error) {
      handleOperatorError(error, 'getOperators')
    }
  }

  public static async createOperator(model: Operator): Promise<Operator> {
    try {
      const { data } = await httpClient.post('/operators', OperatorMapper.toDTO(model))
      return OperatorMapper.toModel(data)
    } catch (error) {
      handleOperatorError(error, 'createOperator')
    }
  }
}
```

## 6. Criar os hooks

**`hooks/use-queries.ts`** — todas as queries da feature:
```ts
import { useQuery } from '@tanstack/react-query'
import { getOperators, getOperatorById } from '../services'

export function useOperatorsQuery() {
  return useQuery({
    queryKey: ['operators'],
    queryFn: getOperators,
  })
}

export function useOperatorQuery(id: string) {
  return useQuery({
    queryKey: ['operators', id],
    queryFn: () => getOperatorById(id),
    enabled: !!id,
  })
}
```

**`hooks/use-mutations.ts`** — todas as mutations da feature:
```ts
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { createOperator } from '../services'

export function useCreateOperatorMutation() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: createOperator,
    meta: { successMessage: 'Operador criado com sucesso!' },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['operators'] })
    },
  })
}
```

## 7. Criar os componentes

**`components/operator-list.tsx`** — consome hooks da feature:
```tsx
import { useOperatorsQuery } from '../hooks/use-queries'

export function OperatorList() {
  const { data, isLoading } = useOperatorsQuery()
  if (isLoading) return <div>Carregando...</div>
  return (
    <ul>
      {data?.map((item) => <li key={item.id}>{item.name}</li>)}
    </ul>
  )
}
```

Componentes com lógica própria (formulários, contextos) têm seu hook **junto ao componente**:

```
components/operator-form/
├── operator-form.tsx
└── use-operator-form.ts   ← hook local, não vai para hooks/
```

## 8. Registrar a rota

Adicionar em `app/routers/` dentro do guard adequado (AuthGuard ou GuestGuard).

## Checklist Final

- [ ] DTO e Model separados em `types/`
- [ ] Mapper criado com `toModel` e `toDTO`
- [ ] Chamadas HTTP apenas em `services/index.ts`
- [ ] Hooks de TanStack Query em `hooks/use-queries.ts` e `hooks/use-mutations.ts`
- [ ] Hooks de componente junto ao componente (`components/<n>/use-<n>.ts`)
- [ ] Componentes sem lógica de negócio
- [ ] Rota registrada com guard correto
- [ ] Arquivos nomeados em `kebab-case`