# Hooks

## Convenção de Arquivos

| Arquivo               | Conteúdo                                          |
|-----------------------|---------------------------------------------------|
| `hooks/use-queries.ts`   | Todas as queries da feature (TanStack Query)   |
| `hooks/use-mutations.ts` | Todas as mutations da feature (TanStack Query) |
| `components/<n>/use-<n>.ts` | Hook local do componente (React Hook Form, contextos, lógica de UI) |

---

## `hooks/use-queries.ts`

Todas as queries da feature em um único arquivo, exportadas individualmente:

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

## `hooks/use-mutations.ts`

Todas as mutations da feature em um único arquivo:

```ts
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { createOperator, updateOperator, deleteOperator } from '../services'

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

export function useUpdateOperatorMutation() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: updateOperator,
    meta: { successMessage: 'Operador atualizado com sucesso!' },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['operators'] })
    },
  })
}

export function useDeleteOperatorMutation() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: deleteOperator,
    meta: { successMessage: 'Operador removido.' },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['operators'] })
    },
  })
}
```

## Mutation com mensagens customizadas via `meta`

O `MutationCache` global lê `mutation.meta` para personalizar o feedback:

| Campo `meta`         | Efeito                                                  |
|----------------------|---------------------------------------------------------|
| `successMessage`     | Toast de sucesso com a mensagem informada               |
| `errorMessage`       | Substitui a mensagem de erro padrão no toast            |
| `suppressErrorToast` | Suprime o toast de erro (para tratamento local de erro) |

---

## Hooks de Componente

Componentes com lógica própria têm seu hook colocado **junto ao componente**, não em `hooks/`. São hooks React comuns — podem usar React Hook Form, Context, `useState`, e consumir `use-queries` e `use-mutations` da feature.

```
components/operator-form/
├── operator-form.tsx
└── use-operator-form.ts   ← hook local, não vai para hooks/
```

Exemplo com React Hook Form e Context para compartilhar estado entre subcomponentes:

```ts
// components/operator-form/use-operator-form.ts
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { createContext, useContext } from 'react'
import { useCreateOperatorMutation, useUpdateOperatorMutation } from '../../hooks/use-mutations'
import { useOperatorsGroupsQuery } from '../../hooks/use-queries'
import { UserFormSchema, type User } from '../../types'

export const OperatorFormContext = createContext<OperatorFormContextValue>(
  {} as OperatorFormContextValue
)

export const OperatorFormProvider = ({ mode, operator, children }) => {
  const { mutateAsync: createOperator } = useCreateOperatorMutation()
  const { mutateAsync: updateOperator } = useUpdateOperatorMutation()
  const { data: groups } = useOperatorsGroupsQuery()

  const form = useForm<User>({
    resolver: zodResolver(UserFormSchema),
    defaultValues: {
      firstName: operator?.firstName || '',
      email: operator?.email || '',
      // ...
    },
  })

  const onSubmit = async (data: User) => {
    if (mode === 'create') await createOperator(data)
    if (mode === 'edit') await updateOperator({ ...data, id: operator?.id || '' })
  }

  return (
    <OperatorFormContext.Provider value={{ mode, form, operator, onSubmit, groups: groups || [] }}>
      {children}
    </OperatorFormContext.Provider>
  )
}

export const useOperatorForm = () => {
  const context = useContext(OperatorFormContext)
  if (!context) throw new Error('useOperatorForm must be used within OperatorFormProvider')
  return context
}
```

---

## Onde colocar cada hook

| Situação                                              | Onde colocar                          |
|-------------------------------------------------------|---------------------------------------|
| Usado por apenas um componente (ex: lógica de form)   | `components/<n>/use-<n>.ts`           |
| Compartilhado por dois ou mais componentes da feature | `hooks/use-<context>.ts`              |
| Queries TanStack Query                                | `hooks/use-queries.ts`                |
| Mutations TanStack Query                              | `hooks/use-mutations.ts`              |

---


## Regras

- `hooks/use-queries.ts` e `hooks/use-mutations.ts` são os únicos pontos de contato com TanStack Query na feature
- **Nunca** usar `onError` nos hooks — o `MutationCache` global já trata todos os erros
- Hook usado por **um único componente** → `components/<n>/use-<n>.ts`
- Hook compartilhado por **dois ou mais componentes** → `hooks/use-<context>.ts`
- Query keys seguem hierarquia: `['operators']`, `['operators', id]`, `['operators', 'detail', id]`
- Hooks nunca chamam `httpClient` diretamente — sempre via service