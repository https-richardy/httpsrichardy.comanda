# Padrões Proibidos

Consulte este arquivo antes de gerar qualquer código. Estes padrões **nunca** devem aparecer.

---

## ❌ useState + useEffect para dados da API

```ts
// PROIBIDO
const [operators, setOperators] = useState([])
useEffect(() => {
  getOperators().then(setOperators)
}, [])

// CORRETO
const { data: operators } = useOperators()
```

---

## ❌ Chamada HTTP direta no componente

```ts
// PROIBIDO
export function OperatorList() {
  useEffect(() => {
    httpClient.get('/operators').then(...)
  }, [])
}

// CORRETO — delegar para hook → service
export function OperatorList() {
  const { data } = useOperators()
}
```

---

## ❌ Chamada HTTP direta no hook

```ts
// PROIBIDO
export function useOperators() {
  return useQuery({
    queryKey: ['operators'],
    queryFn: () => httpClient.get('/operators'), // ← httpClient no hook
  })
}

// CORRETO
export function useOperators() {
  return useQuery({
    queryKey: ['operators'],
    queryFn: getOperators, // ← função do service
  })
}
```

---

## ❌ Consumir DTO diretamente na UI

```ts
// PROIBIDO — componente recebe DTO
export function OperatorCard({ operator }: { operator: OperatorDTO }) {
  return <div>{operator.full_name}</div>
}

// CORRETO — componente recebe Model
export function OperatorCard({ operator }: { operator: Operator }) {
  return <div>{operator.name}</div>
}
```

---

## ❌ Lógica de negócio no componente

```tsx
// PROIBIDO
export function OperatorList() {
  const { data } = useOperators()
  const activeOperators = data?.filter(op => op.status === 'active') // lógica no componente
  const sorted = activeOperators?.sort((a, b) => a.name.localeCompare(b.name))
  return <ul>{sorted?.map(...)}</ul>
}

// CORRETO — lógica no hook ou service
export function useActiveOperators() {
  const { data, ...rest } = useOperators()
  const activeOperators = data?.filter(op => op.isActive).sort(...)
  return { data: activeOperators, ...rest }
}
```

---

## ❌ Importar de outra feature

```ts
// PROIBIDO
import { usePartners } from '@/features/partners/hooks/use-partners'

// CORRETO — se o dado é compartilhado, ele vai para shared/
import { useSharedData } from '@/shared/hooks/use-shared-data'
```

---

## ❌ Side-effects de dados no componente

```tsx
// PROIBIDO
export function CreateOperatorButton() {
  const handleClick = async () => {
    await httpClient.post('/operators', data) // side-effect no componente
  }
}

// CORRETO — usar mutation do hook
export function CreateOperatorButton() {
  const { mutate } = useCreateOperator()
  const handleClick = () => mutate(data)
}
```

---

## ❌ Hook compartilhado colocado junto ao componente pai

Quando um hook é consumido por dois ou mais componentes, ele deve ir para `hooks/`, não ficar junto a um componente.

```ts
// PROIBIDO — hook compartilhado escondido dentro de um componente
components/buy-simulation-form/use-buy-simulation.ts
// ...e o card importa de dentro de outro componente

// CORRETO — hook compartilhado em hooks/
hooks/use-buy-simulation.ts       // form e card consomem daqui
components/buy-simulation-form/use-buy-simulation-form.ts  // exclusivo do form
```

---

## ❌ Hook exclusivo de um componente colocado em `hooks/`

Hooks usados por apenas um componente poluem `hooks/` e devem ficar junto ao componente.

```ts
// PROIBIDO — hook de formulário em hooks/ sendo que só o form o usa
hooks/use-operator-form.ts

// CORRETO — junto ao componente que o usa
components/operator-form/use-operator-form.ts
```

---

## ❌ Service sem tratamento de erro

Toda função de service deve envolver a chamada HTTP em `try/catch` e chamar o error handler.

```ts
// PROIBIDO — erro não tratado, sobe como AxiosError bruto
export async function createOperator(model: Operator): Promise<Operator> {
  const { data } = await httpClient.post('/operators', OperatorMapper.toDTO(model))
  return OperatorMapper.toModel(data)
}

// CORRETO — erro capturado e mapeado pelo error handler
export async function createOperator(model: Operator): Promise<Operator> {
  try {
    const { data } = await httpClient.post('/operators', OperatorMapper.toDTO(model))
    return OperatorMapper.toModel(data)
  } catch (error) {
    handleOperatorError(error, 'createOperator')
  }
}
```

---

## ❌ Criar componentes primitivos manualmente em vez de usar shadcn/ui

Antes de criar qualquer componente em `shared/components/ui/`, verificar se existe equivalente no shadcn/ui e instalá-lo via CLI.

```tsx
// PROIBIDO — criar Button do zero quando shadcn já tem
// shared/components/ui/button.tsx (criado manualmente)
export function Button({ label, onClick }) {
  return <button onClick={onClick}>{label}</button>
}

// CORRETO — instalar via CLI e usar o componente gerado
// pnpm dlx shadcn@latest add button
import { Button } from '@/shared/components/ui/button'
```

---

## ❌ Importar store diretamente na camada `infra/`

A infra não conhece stores. Dependências como token e callbacks de auth são injetadas via parâmetro pelo bootstrap em `app/`.

```ts
// PROIBIDO — infra acoplada ao store
import { useAuthStore } from '@/features/auth/store'

export const httpClient = axios.create({ baseURL: env.VITE_GATEWAY_API })

httpClient.interceptors.request.use((config) => {
  const token = useAuthStore.getState().token // ← infra importando store
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

// CORRETO — dependência injetada via parâmetro
export const setupHttpClientInterceptors = (
  getToken: () => string | null,
  onUnauthorized: () => void
) => {
  httpClient.interceptors.request.use((config) => {
    const token = getToken() // ← injetado pelo bootstrap
    if (token) config.headers.Authorization = `Bearer ${token}`
    return config
  })
}
```

---

## ❌ `onError` em hooks de mutation

O `MutationCache` global já trata todos os erros e exibe toasts. Adicionar `onError` no hook duplica o tratamento.

```ts
// PROIBIDO
useMutation({
  mutationFn: createOperator,
  onError: (error) => {
    toast.error('Erro ao criar operador') // duplica o MutationCache global
  },
})

// CORRETO — usar meta para customizar a mensagem
useMutation({
  mutationFn: createOperator,
  meta: { errorMessage: 'Não foi possível criar o operador.' },
  onSuccess: () => {
    queryClient.invalidateQueries({ queryKey: ['operators'] })
  },
})
```

---

## ❌ alert() ou console.error() como feedback de UI

```ts
// PROIBIDO
catch (error) {
  alert('Erro ao salvar')
  console.error(error)
}

// CORRETO
catch (error) {
  const message = handleOperatorError(error)
  toast.error(message)
}
```

---

## ❌ Tipos `any` para dados da API

```ts
// PROIBIDO
const { data } = await httpClient.get('/operators') as any

// CORRETO — tipar com DTO e mapear para Model
const { data }: { data: OperatorDTO[] } = await httpClient.get('/operators')
return data.map(toModel)
```