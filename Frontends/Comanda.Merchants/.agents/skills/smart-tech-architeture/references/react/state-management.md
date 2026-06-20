# Gestão de Estado

## Regra de Decisão

```
Dado vem da API?
  → TanStack Query

Dado é global de UI (auth, tema, menu)?
  → Zustand

Dado é local do componente (dropdown, input)?
  → useState / useReducer
```

## 1. Server State — TanStack Query

Para qualquer dado que vem do backend.

```ts
const { data, isLoading, error } = useOperators()
```

**Nunca** usar `useState + useEffect` para buscar dados da API.

## 2. Estado Global de UI — Zustand

Para estado compartilhado entre features que não vem da API.

```ts
// store de autenticação
interface AuthStore {
  token: string | null
  setToken: (token: string) => void
  clearToken: () => void
}

export const useAuthStore = create<AuthStore>((set) => ({
  token: null,
  setToken: (token) => set({ token }),
  clearToken: () => set({ token: null }),
}))
```

Casos de uso: token de auth, tema (dark/light), estado do menu lateral, preferências do usuário.

## 3. Estado Local — React Hooks

Para estado que pertence exclusivamente ao componente.

```ts
const [isOpen, setIsOpen] = useState(false)
const [selectedId, setSelectedId] = useState<string | null>(null)
```

Casos de uso: abertura de modal, item selecionado, valor de input não controlado por form.

## Anti-pattern mais comum

```ts
// ❌ Errado — useState para dados da API
const [operators, setOperators] = useState([])
useEffect(() => {
  getOperators().then(setOperators)
}, [])

// ✅ Correto — TanStack Query
const { data: operators } = useOperators()
```