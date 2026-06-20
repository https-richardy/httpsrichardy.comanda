# Implementação de Páginas

## Estrutura Padrão

Páginas ficam em `features/<n>/components/` e seguem sempre a mesma estrutura:
- `PageHeader` com o título da seção
- Container com padding padrão
- Bloco de título + descrição da página
- Componente(s) de conteúdo

```tsx
export const ReportsPage = () => {
  return (
    <>
      <PageHeader title="Relatórios" />
      <div className="flex flex-col gap-4 py-4 md:gap-6 md:py-6 px-6">
        <div className="flex flex-col gap-2 pb-6">
          <h2 className="text-2xl font-semibold">Gerenciar relatórios</h2>
          <p className="text-muted-foreground">
            Gerencie de forma centralizada os relatórios.
          </p>
        </div>
        <ReportViewer />
      </div>
    </>
  )
}
```

## Anatomia

| Elemento                  | Responsabilidade                                                        |
|---------------------------|-------------------------------------------------------------------------|
| `<PageHeader title="..." />` | Cabeçalho da seção — componente de `shared/components/common/`       |
| Container `div`           | Padding e espaçamento padrão — nunca alterar as classes base            |
| Bloco `h2` + `p`          | Título e descrição contextual da página                                 |
| Componentes de conteúdo   | Delegam toda a lógica para seus próprios hooks — a página não orquestra dados |

## Classes de Layout Padrão

O container da página usa sempre estas classes — não customizar por feature:

```
flex flex-col gap-4 py-4 md:gap-6 md:py-6 px-6
```

## Regras

- Páginas são componentes **sem lógica** — não chamam hooks de dados, não fazem fetch
- Toda orquestração de dados fica nos componentes filhos e seus hooks
- O nome do componente segue o padrão `<NomeDaFeature>Page`
- Exportação **nomeada** — nunca `export default`
- Registrar a página em `app/routers/` dentro do guard adequado