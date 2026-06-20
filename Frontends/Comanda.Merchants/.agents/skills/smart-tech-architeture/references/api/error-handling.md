# Tratamento de Erros

## Três Níveis de Tratamento

### 1. Validação — Zod

Erros de formulário antes de chegar na API:

```ts
import { z } from 'zod'

export const operatorSchema = z.object({
  name: z.string().min(1, 'Nome obrigatório'),
  email: z.string().email('E-mail inválido'),
})

export type OperatorFormData = z.infer<typeof operatorSchema>
```

Integração com React Hook Form:
```tsx
const { register, handleSubmit, formState: { errors } } = useForm<OperatorFormData>({
  resolver: zodResolver(operatorSchema),
})
```

### 2. Domínio — `services/error-handler.ts`

Cada feature tem um error handler que intercepta erros da API, mapeia status HTTP para mensagens legíveis e **sempre relança** com `throw new Error(message)` — nunca retorna string.

O tipo de retorno é `never` porque a função obrigatoriamente lança uma exceção.

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

Uso no service — o erro relançado sobe para o `MutationCache` global:

```ts
export async function createOperator(model: Operator): Promise<Operator> {
  try {
    const { data } = await httpClient.post('/operators', OperatorMapper.toDTO(model))
    return OperatorMapper.toModel(data)
  } catch (error) {
    handleOperatorError(error, 'createOperator')
  }
}
```

### 3. Mutations — `MutationCache` Global

O erro relançado pelo error handler é capturado automaticamente pelo `MutationCache`, que exibe o toast usando `error.message`:

```ts
// infra/query-client.ts — configuração global (não repetir nas features)
export const queryClient = new QueryClient({
  mutationCache: new MutationCache({
    onError: (error, _variables, _context, mutation) => {
      if (error instanceof AxiosError && error.response?.status === 401) return

      if (mutation.meta?.suppressErrorToast) return

      const customMessage = mutation.meta?.errorMessage
      const backendMessage = error instanceof AxiosError ? error.message : 'Erro desconhecido'

      toast.error(customMessage || `Erro na operação: ${backendMessage}`)
    },
    onSuccess: (_data, _variables, _context, mutation) => {
      if (mutation.meta?.successMessage) {
        toast.success(mutation.meta.successMessage)
      }
    },
  }),
  defaultOptions: {
    queries: {
      retry: false,
      refetchOnWindowFocus: false,
    },
  },
})
```

**Customizando o feedback por operação via `meta`:**

```ts
return useMutation({
  mutationFn: createOperator,
  meta: {
    successMessage: 'Operador criado com sucesso!',
    // errorMessage sobrescreve a mensagem do error handler se necessário
    // suppressErrorToast: true — suprime o toast completamente
  },
  onSuccess: () => {
    queryClient.invalidateQueries({ queryKey: ['operators'] })
  },
})
```

| Campo `meta`         | Efeito                                                   |
|----------------------|----------------------------------------------------------|
| `successMessage`     | Toast de sucesso com a mensagem informada                |
| `errorMessage`       | Sobrescreve a mensagem do error handler no toast         |
| `suppressErrorToast` | Suprime o toast — usar apenas quando erro é tratado localmente |

### 4. Infraestrutura — Interceptores via Bootstrap

O 401 é tratado no interceptor de resposta, configurado no bootstrap da aplicação. A infra **não importa** o store diretamente — recebe o callback `onUnauthorized` como parâmetro injetado:

```ts
// app/bootstrap.ts
setupHttpClientInterceptors(getToken, () => {
  const isLoggingOut = useAuthStore.getState().status === 'unauthenticated'
  if (!isLoggingOut) {
    window.dispatchEvent(new CustomEvent('on-session-expired'))
    useAuthStore.getState().logout()
  }
})
```

## Fluxo Completo de um Erro de Mutation

```
Service (try/catch)
  → handleOperatorError() — mapeia status HTTP → throw new Error(message)
    → MutationCache.onError — exibe toast com error.message
```

## Regra de Responsabilidade

| Tipo de erro                        | Onde tratar                          |
|-------------------------------------|--------------------------------------|
| Campo inválido no formulário        | Zod schema                           |
| Erro de domínio da API              | `services/error-handler.ts`          |
| Exibição do toast de erro           | `MutationCache` global (automático)  |
| Mensagem customizada por operação   | `meta.errorMessage` no hook          |
| HTTP 401 (sessão expirada)          | Interceptor em `infra/api/`          |

## Feedback ao Usuário

Todo erro visível ao usuário é exibido via **toast** pelo `MutationCache`. Nunca usar `alert()` ou `console.error()` como feedback de UI, nem duplicar o tratamento com `onError` no hook.