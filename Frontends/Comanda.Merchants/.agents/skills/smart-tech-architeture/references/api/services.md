# Services e Clientes HTTP

## Estrutura do Service

Todo service fica em `features/<n>/services/index.ts`. Cada método envolve a chamada HTTP em `try/catch` e delega ao error handler, que mapeia os erros e relança:

```ts
import { httpClient } from '@/infra/api'
import { OperatorMapper } from './mapper'
import { handleOperatorError } from './error-handler'
import type { Operator } from '../types/operator.model'

export  class OperatorService {
  public static async getOperators(): Promise<Operator[]> {
    try {
      const { data } = await httpClient.get('/operators')
      return OperatorMapper.toModelList(data)
    } catch (error) {
    handleOperatorError(error, 'getOperators')
    }
  }

  public static async getOperatorById(id: string): Promise<Operator> {
    try {
      const { data } = await httpClient.get(`/operators/${id}`)
      return OperatorMapper.toModel(data)
    } catch (error) {
      handleOperatorError(error, 'getOperatorById')
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

  public static async updateOperator(id: string, model: Operator): Promise<Operator> {
    try {
      const { data } = await httpClient.put(`/operators/${id}`, OperatorMapper.toDTO(model))
      return OperatorMapper.toModel(data)
    } catch (error) {
      handleOperatorError(error, 'updateOperator')
    }
  }

  public static async deleteOperator(id: string): Promise<void> {
    try {
      await httpClient.delete(`/operators/${id}`)
    } catch (error) {
      handleOperatorError(error, 'deleteOperator')
    }
  }
}
```

## Cliente HTTP Base (`infra/api/`)

O `httpClient` é criado sem interceptores. Interceptores são **injetados externamente** via `setupHttpClientInterceptors`, que recebe o `getToken` e o `onUnauthorized` como parâmetros — sem acoplar a infra ao store de auth.

```ts
// infra/api/gateway-api.ts
export const httpClient = axios.create({
  baseURL: env.VITE_GATEWAY_API,
})

export const setupHttpClientInterceptors = (
  getToken: () => string | null,
  onUnauthorized: () => void
) => {
  httpClient.interceptors.request.use(
    (config) => {
      const token = getToken()
      if (token) config.headers.Authorization = `Bearer ${token}`
      return config
    },
    (error) => Promise.reject(error)
  )

  httpClient.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response?.status === 401) onUnauthorized()
      return Promise.reject(error)
    }
  )
}
```

A função é chamada uma única vez no bootstrap da aplicação (`app/`):

```ts
// app/bootstrap.ts
import { setupHttpClientInterceptors } from '@/infra/api/gateway-api'
import { useAuthStore } from '@/features/auth/store'

export const bootstrapApp = () => {
  const getToken = () => useAuthStore.getState().token

  setupHttpClientInterceptors(getToken, () => {
    const isLoggingOut = useAuthStore.getState().status === 'unauthenticated'
    if (!isLoggingOut) {
      window.dispatchEvent(new CustomEvent('on-session-expired'))
      useAuthStore.getState().logout()
    }
  })
}
```

## Regras

- `httpClient` só é importado em `services/index.ts` e `infra/`
- Services sempre retornam Models (nunca DTOs)
- Services sempre recebem Models como parâmetro (nunca DTOs)
- **`infra/` nunca importa stores ou hooks** — dependências são injetadas via parâmetro
- A configuração de interceptores é responsabilidade da camada `app/` (bootstrap), não da infra