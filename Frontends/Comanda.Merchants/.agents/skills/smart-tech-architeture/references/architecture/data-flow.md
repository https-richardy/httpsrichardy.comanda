# Fluxo de Dados

## Leitura (Queries)

```
API → Service → Mapper → React Query → Hook → Component
```

1. **API** retorna um DTO
2. **Service** chama `httpClient` e repassa para o mapper
3. **Mapper** converte DTO → Model
4. **React Query** armazena o Model em cache
5. **Hook** expõe os dados via `useQuery`
6. **Component** consome o hook

## Escrita (Mutations)

```
Component → Hook → Service → Mapper → API
```

1. **Component** chama a função do hook (ex: `createOperator(data)`)
2. **Hook** invoca a mutation do TanStack Query
3. **Service** recebe o Model, converte para DTO via mapper
4. **Mapper** converte Model → DTO
5. **API** recebe o DTO

Após sucesso, o hook invalida a query correspondente:
```ts
onSuccess: () => {
  queryClient.invalidateQueries({ queryKey: ['operators'] })
}
```

## Tempo Real (SSE / SignalR)

```
Backend Event → Infra Client → React Query Cache → Component re-render
```

- O cliente SSE/SignalR **nunca** usa `setState` diretamente
- Atualiza o cache via `queryClient.setQueryData` ou `queryClient.invalidateQueries`
- O componente re-renderiza automaticamente por ser subscriber do cache